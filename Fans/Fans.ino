#include <FastLED.h>
#include <EEPROM.h>
#include <Servo.h>

#define LED_PIN     D8
#define NUM_LEDS    20
#define NUM_StateLEDS 5
#define LED_TYPE    WS2811
#define COLOR_ORDER GRB
#define UPDATES_PER_SECOND 100

int BRIGHTNESS = 64 ;
CRGB leds[NUM_LEDS];
extern CRGBPalette16 myRedWhiteBluePalette;
extern const TProgmemPalette16 myRedWhiteBluePalette_p PROGMEM;

#define ServoTop_PIN D1
#define ServoBottom_PIN D2
#define ServoMotors_PIN D3

Servo top_servo;
Servo bottom_servo;
Servo fans_servo;

#define motors_PIN D7

bool servoRotation1 =0;

#define Switch1 D5
#define Switch2 D6
bool FanDirection = 0;
bool FanPower = 0;

#define button_s  D0
#define buttons  A0
int keyValue  =  0; 

int speed = 2;

const int minValue = 35; 

int address = 0;



void setup() {

  Serial.begin(9600);

  EEPROM.begin(512);

  byte value;
  value = EEPROM.read(address);
  if(value != 0){
    if(value<=15){
      speed = value;
    }
    else{
      speed = 2;
    }
  }
  
  Serial.print(address);
  Serial.print("\t");
  Serial.print(value, DEC);
  Serial.println();


  //Светодиоды

  FastLED.addLeds<LED_TYPE, LED_PIN, COLOR_ORDER>(leds, NUM_LEDS).setCorrection( TypicalLEDStrip );
  FastLED.setBrightness(  BRIGHTNESS );

  //Сервы заслонок
  top_servo.attach(ServoTop_PIN);
  bottom_servo.attach(ServoBottom_PIN);

  //Серва переключателя потока
  fans_servo.attach(ServoMotors_PIN);

  //питание вентиляторов
  pinMode(Switch1, OUTPUT); 
  pinMode(Switch2, OUTPUT); 
  pinMode(motors_PIN, OUTPUT);

  //Кнопки
  pinMode(buttons, INPUT);

  SetSpeedMotors();
  SetServoPosition();

}

void loop() {
  // put your main code here, to run repeatedly:

  UpdateKeyValue();

  //leds.setBrightness(BRIGHTNESS);

  ShowLedState();

  SetDirection();
 

}

void SetSpeedMotors(){  
  analogWrite(motors_PIN, speed*17);
  Serial.println("Set speed: " + String(speed*17));
}

void SetServoPosition(){  
  if(servoRotation1){  
    top_servo.write(minValue);
    bottom_servo.write(270);
  }
  else{    
    top_servo.write(180);
    bottom_servo.write(minValue);
  }
}

void UpdateKeyValue(){
  int newKeyValue = GetKeyValue(); // Получаем актуальное состояние кнопок с коррекцией дребезга
  if (keyValue != newKeyValue) {  // Если новое значение не совпадает со старым - реагируем на него
    keyValue = newKeyValue;       // Актуализируем переменную хранения состояния
    if (keyValue > 0) {           // Если значение больше 0, значит кнопка нажата
      Serial.println("Key pressed: " + String(keyValue));

      if(keyValue == 1){
        if(FanPower){
          FanPower = 0;
        }
        else{
          FanPower = 1;
        }
      }

      if(keyValue == 2){
        if(FanDirection){
            FanDirection = 0;
        }
        else{
          FanDirection = 1;
        }
      }

      if(keyValue == 3){
        if(servoRotation1){
          servoRotation1=0;
        }
        else{
          servoRotation1 =1;
        }        
        Serial.println("SetServoPosition: "+String(servoRotation1));
        SetServoPosition();
      }
      
      if(keyValue == 4){
        if(speed>1){
          speed--;

          Serial.println("Speed: "+String(speed));

          SetSpeedMotors();

          EEPROM.write(address, speed);
          EEPROM.commit();

        }
      }
      
      if(keyValue == 5){
        if(speed<15){
          speed++;
          SetSpeedMotors();
          Serial.println("Speed: "+String(speed));

          EEPROM.write(address, speed);
          EEPROM.commit();
        }
      }
      


    }
    else {                        // Если 0, то состояние покоя
      Serial.println("all keys are not pressed");
    }
  }
}

int GetKeyValue() {         // Функция устраняющая дребезг
  static int   oldKeyValue; // Переменная для хранения предыдущего значения состояния кнопок
  static long  lastChange;  // Переменная для хранения времени последнего изменения состояния
  int actualKeyValue = analogRead(buttons);  // Получаем актуальное состояние
  
  actualKeyValue = GetButtonNumberByValue(actualKeyValue);

  if ((actualKeyValue != oldKeyValue) && (millis() - lastChange > 100)) { // Пришло новое значение, и с последнего изменения прошло достаточно времени
    oldKeyValue = actualKeyValue; // Запоминаем новое значение
    lastChange = millis();        // Обнуляем таймер
  }
  return    oldKeyValue;          // Отправляем старое, либо уже модифицированное новое значение

}

int GetButtonNumberByValue(int value) {   // Новая функция по преобразованию кода нажатой кнопки в её номер
  int values[6] = {15, 229, 283, 367, 546, 1024};
  int error     = 10;                     // Величина отклонения от значений - погрешность
  for (int i = 0; i <= 5; i++) {
    // Если значение в заданном диапазоне values[i]+/-error - считаем, что кнопка определена
    if (value <= values[i] + error && value >= values[i] - error) return i;
  }
  return 0;                              // Значение не принадлежит заданному диапазону
}


void SetDirection(){
  if(FanPower){   
      if(FanDirection){

        fans_servo.write(0);

        digitalWrite(Switch1, HIGH);
        digitalWrite(Switch2, LOW);
      }
      else{    

        fans_servo.write(180);

        digitalWrite(Switch2, HIGH);
        digitalWrite(Switch1, LOW);
      }    
  }
  else{    

      fans_servo.write(90);
      
      digitalWrite(Switch1, HIGH);
      digitalWrite(Switch2, HIGH);
  }
}

void ShowLedState(){
  static int oldSpeedValue;
  static bool oldPower;
  static bool oldFanDirection;
  static bool oldservoRotation1;

  if ((speed != oldSpeedValue) || (FanPower != oldPower) || (oldFanDirection != FanDirection) || (oldservoRotation1 != servoRotation1)){
    oldSpeedValue= speed;
    oldPower=FanPower;
    oldFanDirection = FanDirection;
    oldservoRotation1 = servoRotation1;

    int color = (speed*255)/15;

    for(int i=0; i<NUM_LEDS ; i++) { 
        if(i>NUM_LEDS-NUM_StateLEDS-1){
          switch (i) {
            case 19:
              if(FanPower){
                leds[i] = CRGB(0,   60,   0);
              }
              else{
                leds[i] = CRGB(60,   0,   0);
              }
            break;

            
            case 18: 
              if(!servoRotation1){
                leds[i] = CRGB(0,   60,   0);
              }
              else{
                leds[i] = CRGB(50,   0,   0);
              }
            break;

            case 17: 
              if(servoRotation1){
                leds[i] = CRGB(0,   60,   0);
              }
              else{
                leds[i] = CRGB(50,   0,   0);
              }
            break;


            case 16:
              if(!FanDirection){
                leds[i] = CRGB(0,   60,   0);
              }
              else{
                leds[i] = CRGB(60,   0,   0);
              }
            break;
            
            case 15:
              if(FanDirection){
                leds[i] = CRGB(0,   60,   0);
              }
              else{
                leds[i] = CRGB(50,   0,   0);
              }
            break;
          }
          
        }
        else{
          if(FanPower){
            if(i<=speed-1){
              leds[i] = CRGB(255-color,   color,   0);
            }
            else{
              leds[i] = CRGB(23,   0,   37);
            }          
          }
          else{            
              leds[i] = CRGB(23,   0,   37);            
          }
        }
        FastLED.show(); // отправляем информацию на ленту 
    }    
  }
}

