#include <Wire.h> 
#include <LiquidCrystal_I2C.h>
#include <DHT.h>
#include <ESP32Servo.h>
#include <Wire.h>
LiquidCrystal_I2C lcd(0x27, 16, 2);
#define BACKLIGHT_PIN 13
#define water 1
#define button 8
#define DHTPIN 7  
#define DHTTYPE DHT11  
DHT dht(DHTPIN, DHTTYPE);
Servo servo1;
const int magnet_switch = 4;   
const int alarm_sound = 12;
int rain;
int h;
int t; 
boolean button1;
boolean but_flag;
String weather;
void setup()
{
  pinMode(alarm_sound, OUTPUT);
  servo1.attach(2);
  pinMode(BACKLIGHT_PIN , OUTPUT);
  pinMode(water, INPUT);
  pinMode(DHTTYPE,INPUT);
   pinMode(8, INPUT_PULLUP);
  digitalWrite(BACKLIGHT_PIN, HIGH);
  Serial.begin(9600);
  lcd.begin();
  dht.begin();
  lcd.home ();
  lcd.print("hello");
    pinMode(magnet_switch, INPUT_PULLUP);
}
void alarm(int buzzer)
{
   int alarm_read = digitalRead(magnet_switch);
   alarm_read = 1;
   while (alarm_read == 1)
   { // Run for 5 seconds
    alarm_read = digitalRead(magnet_switch);
    Serial.println(alarm_read);
    tone(buzzer, 4000); // Set the buzzer to a frequency (adjust as needed)
    delay(100); // Play the sound for 100 milliseconds (adjust as needed)
    noTone(buzzer); // Stop the sound
    delay(100); // Pause for 100 milliseconds (adjust as needed)
   }
}
void win_close()
{
  servo1.write(95);
  //servo1.write(0);
  delay(1000);
  servo1.detach();
  int alarm_read = digitalRead(magnet_switch);
  if (alarm_read == HIGH )
    {
      lcd.clear();
      lcd.setCursor(3, 0);
      lcd.print("!! ALARM !!");
      but_flag = 0;
      alarm(alarm_sound);
    }
}

void win_open()
{
    servo1.attach(2);
    //servo1.write(360);
    servo1.write(80);
    delay(1000); 
  }
  
  void TemparaturePrtint(int t,int h)
  {
     lcd.setCursor(0, 0);
      lcd.println("Condition inside"); 
      lcd.setCursor(10,0) ;
      lcd.setCursor(0, 1);
      lcd.print("T:");
      lcd.print(t);
      lcd.print("C");
      lcd.setCursor(6, 1);
      lcd.setCursor(11, 1);
      lcd.print("H:");
      lcd.print(h);
      lcd.print("%");
      delay(100);
  }
  
  void printlong(String text,int row,int longtext)
    {
      lcd.setCursor(0,row);
      lcd.println(text);
        for (int i = 0; i < longtext ;i++)
        {
          lcd.scrollDisplayLeft();
          delay(300);
        }
    }
  void loop()
  {
    button1 = !digitalRead(button);
    rain = analogRead(water);
    h = dht.readHumidity();
    t = dht.readTemperature();
    TemparaturePrtint(t, h);
    if (button1 == 1)
    {
      but_flag = !but_flag;
      delay(200);
    }
  
    if (but_flag == 0)
    {
      win_close();
    }
    else 
    { //butflag == 1  open window 
      if (rain > 250 )
      {
        lcd.clear();
         tone(alarm_sound,4000,100);
        printlong("Window close while raining",0,10);
        lcd.setCursor(10,1);
        tone(alarm_sound,4000,600);
        lcd.print("Manual open only");
        win_close();
        lcd.clear();
        but_flag = 0;
        TemparaturePrtint(t,h);
      }
      else 
      {
        win_open();
      }
  }
}
 
  





  
