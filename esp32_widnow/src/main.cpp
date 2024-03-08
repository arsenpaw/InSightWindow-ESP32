#include <Arduino.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <ESP32Servo.h>
#include <Wire.h>
#include <WiFi.h>
WiFiServer server(80);
#define magnet_switch 19  
#define alarm_sound 26
#define servo_pin  16
#define water 25
#define button 17
#define DHTPIN 33  
#define DHTTYPE DHT11  
DHT dht(DHTPIN, DHTTYPE);
Servo servo1;
int rain;
int h;
int t; 
boolean button1;
boolean but_flag;
String header;
boolean is_protected = false;
const char* ssid     = "ESP32-Network";
const char* password = "11111111";
unsigned long currentTime = millis();
// Previous time
unsigned long previousTime = 0;
// Define timeout time in milliseconds
const long timeoutTime = 2000;
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
    Serial.println("");
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

void win_close()
{
  Serial.print("FUNC CLOSE");
  Serial.println(" ");
  servo1.attach(servo_pin);
  servo1.write(110);
  delay(1200);
  servo1.detach();
  if (is_window_open() == 1)
    {
      warning();
    }
}

void win_open()
{
    Serial.print("FUNC OPEN");
    Serial.println(" ");
    servo1.attach(servo_pin);
    servo1.write(72);
    delay(1200); 
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
      win_close();
    }
    else if ((but_flag == 1 && button1 == 1))
    { //butflag == 1  open window 
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
 WiFiClient client = server.available();   // Listen for incoming clients

  if (client) {                             // If a new client connects,
    currentTime = millis();
    previousTime = currentTime;
    Serial.println("New Client.");          // print a message out in the serial port
    String currentLine = "";                // make a String to hold incoming data from the client

    while (client.connected() && currentTime - previousTime <= timeoutTime) {
      // loop while the client's connected
      currentTime = millis();
      if (client.available()) {             // if there's bytes to read from the client,
        char c = client.read();             // read a byte, then
        Serial.write(c);                    // print it out the serial monitor
        header += c;
        if (c == '\n') {                    // if the byte is a newline character
          // if the current line is blank, you got two newline characters in a row.
          // that's the end of the client HTTP request, so send a response:
          if (currentLine.length() == 0) {
            // HTTP headers always start with a response code (e.g. HTTP/1.1 200 OK)
            // and a content-type so the client knows what's coming, then a blank line:
            client.println("HTTP/1.1 200 OK");
            client.println("Content-type:text/html");
            client.println("Connection: close");
            client.println();

            // turns the GPIOs on and off
            if (header.indexOf("GET /16/on") >= 0) {
              win_open();
          // turns the LED on
            } else if (header.indexOf("GET /16/off") >= 0) {
              win_close();
            }
            
          

            // Display the HTML web page
            client.println("<!DOCTYPE html><html>");
            client.println("<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            client.println("<link rel=\"icon\" href=\"data:,\">");
            // CSS to style the on/off buttons
            client.println("<style>html { font-family: monospace; display: inline-block; margin: 0px auto; text-align: center;}");
            client.println(".button { background-color: yellowgreen; border: none; color: white; padding: 16px 40px;");
            client.println("text-decoration: none; font-size: 32px; margin: 2px; cursor: pointer;}");
            client.println(".button2 {background-color: gray;}</style></head>");

            client.println("<body><h1>ESP32 Web Server</h1>");
            client.println("<p>Control LED State</p>");

            if (is_window_open() == false) {
              client.println("<p><a href=\"/16/on\"><button class=\"button\">ON</button></a></p>");
            } else {
              client.println("<p><a href=\"/16/off\"><button class=\"button button2\">OFF</button></a></p>");
            }
            

            // The HTTP response ends with another blank line
            client.println();
            // Break out of the while loop
            break;
          } else { // if you got a newline, then clear currentLine
            currentLine = "";
          }
        } else if (c != '\r') {  // if you got anything else but a carriage return character,
          currentLine += c;      // add it to the end of the currentLine
        }
      }
    }
    // Clear the header variable
    header = "";
    // Close the connection
    client.stop();
    Serial.println("Client disconnected.");
    Serial.println("");
  }

  }

  





  