#include <Arduino.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <ESP32Servo.h>
#include <Wire.h>
#include <WiFi.h>
#include <string.h>
#include <HTTPClient.h>
#include <iostream>
#include <map>
#include <unordered_map>

WiFiServer server(80);
#define magnet_switch 19  
#define alarm_sound 26
#define servo_pin  16
#define water 25
#define button 17
#define DHTPIN 33  
#define DHTTYPE DHT11  
DHT dht(DHTPIN, DHTTYPE);
std::string str;
Servo servo1;
int rain;
int h;
int t; 
boolean button1;
boolean but_flag;
String header;
String url = "http://192.168.4.2:81/WindowStatus";
boolean is_protected = false;
char jsonOutput[128];
const char* ssid     = "ESP32-Network";
const char* password = "11111111";
unsigned long currentTime = millis();
unsigned long previousTime = 0;
const long timeoutTime = 10000;
void setup()
{
  Serial.begin(115200);
  pinMode(alarm_sound, OUTPUT);
  servo1.attach(servo_pin);
  pinMode(water, INPUT);
  pinMode(DHTPIN,INPUT);
  pinMode(button, INPUT_PULLUP);
  pinMode(magnet_switch, INPUT_PULLUP);
  dht.begin();
  WiFi.softAP(ssid,password);
  Serial.println("Connecting");
  Serial.println("IP address: ");
  Serial.println(WiFi.softAPIP());
  server.begin();
  
}

  
boolean is_window_open()
{
  int alarm_read = digitalRead(magnet_switch);
   Serial.print("IS WINDOW OPEN ");
   Serial.print(alarm_read);
   Serial.println(" ");
  return alarm_read;
}
void alarm()
{ 
  int buzzer = alarm_sound;
  Serial.print("FUNC ALARM");
  Serial.println(" ");
   while (is_window_open() == 1)
   { // Run for 5 seconds
    tone(buzzer, 4000); // Set the buzzer to a frequency (adjust as needed)
    delay(100); // Play the sound for 100 milliseconds (adjust as needed)
    noTone(buzzer); // Stop the sound
    delay(100); // Pause for 100 milliseconds (adjust as needed)
   }
}
void warning()
{ 
  Serial.print("FUNC warning");
  Serial.println(" ");
   int buzzer = alarm_sound;

   while (is_window_open() == 1)
   { // Run for 5 seconds
    Serial.print("FUNC warning");
    tone(buzzer, 5000); // Set the buzzer to a frequency (adjust as needed)
    delay(1000); // Play the sound for 100 milliseconds (adjust as needed)
    noTone(buzzer); // Stop the sound
    delay(500); // Pause for 100 milliseconds (adjust as needed)
     }
}
void short_warning()
{ 
  Serial.print("TEMP warning");
  Serial.println(" ");
  int buzzer = alarm_sound;
  for (int i = 0; i < 4; i++)
  {
    tone(buzzer, 1500);
    delay(200);
    noTone(buzzer);
    delay(200);
  }
  tone(buzzer, 1800,1000);
  noTone(buzzer);
}

boolean win_close()
{
  Serial.print("FUNC CLOSE");
  Serial.println(" ");
  servo1.attach(servo_pin);
  servo1.write(110);
  delay(1000);
  servo1.detach();
  if (is_window_open() == 1)
    {
      return false;
    }
  else
  {
      return true;
  }
}

void win_open()
{
    Serial.print("FUNC OPEN");
    Serial.println(" ");
    servo1.attach(servo_pin);
    servo1.write(72);
    delay(1000); 
    servo1.detach();
}
  
  void loop()
  {
    is_window_open();
    button1 = !digitalRead(button);
    rain = analogRead(water);
    h = dht.readHumidity();
    t = dht.readTemperature();
    
  
    Serial.print(F("Humidity: "));
    Serial.print(h);
    Serial.print(F("%  Temperature: "));
    Serial.print(t);
    Serial.print(F("°C "));
    Serial.println(" ");
    Serial.print("BUTTON ");
    Serial.print(button1);
    Serial.println(" ");
    Serial.print("BUT FLAG ");
    Serial.print(but_flag);
    Serial.println(" ");
    Serial.print("WATER LEVEL ");
    Serial.print(rain);
    Serial.println(" ");
    if (button1 == 1)
    {
      but_flag = !but_flag;
      delay(200);
    }
  
    if (but_flag == 0 && button1 == 1)
    {
      boolean is_closed =  win_close();
    }
    else if ((but_flag == 1 && button1 == 1))
    { 
      win_open();
    }
    if (rain > 250 && is_window_open() == true)
    {
      Serial.print("RAIND DETECTED ");
      short_warning();
      delay(100);
      win_close();
    }
    if (is_window_open() == 1 && but_flag ==0 && is_protected == true )
    {
      alarm();
    }
    unsigned long currentMillis = millis();
    WiFiClient client = server.available();   // Listen for incoming clients
    if (WiFi.softAPgetStationNum() > 0 && currentMillis - previousTime > timeoutTime) 
    {
      previousTime = currentMillis;
      Serial.println("TEST");
      Serial.println("User connected to ESP32's local WiFi");
        HTTPClient client;
          client.begin(url.c_str());
          client.addHeader("Content-Type", "application/json");
          String temperatureStr = "\"" + String(t) + "\"";
          String humidityStr = "\"" + String(h) + "\"";
          String isOpenStr = is_window_open() ? "true" : "false";
          isOpenStr ="\"" + isOpenStr + "\"";
          String waterLevelStr = "\"" + String(rain)+ "\"";
          String is_protectedStr = is_protected ? "true" : "false";
          is_protectedStr ="\"" + is_protectedStr + "\"";
          String jsonString = "{\"temparature\": " + temperatureStr + ", \"humidity\": " + humidityStr + ", \"isOpen\": " + isOpenStr + ", \"waterLevel\": " + waterLevelStr + ", \"isProtected\": " + is_protectedStr + "}";
          Serial.println("JSON STRING: ");
          Serial.println(jsonString);
          int httpResponseCode = client.POST(jsonString);
          Serial.println("HTTP POST  CODE: ");
          Serial.println(httpResponseCode);
          if (httpResponseCode > 0) {
            String response = client.getString();
            Serial.println("Response:");
            Serial.println(response);
          } 
          else 
          {
            Serial.println("Error: No response received");
            
          }
          client.end();
          
    }
   // delay(500);
  }


  





  