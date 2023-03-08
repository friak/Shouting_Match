#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick; //Create instance of this object


const int sampleWindow = 150; // Sample window width in milliseconds
unsigned int shout;
int ledPin = 9; //debug leds

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);


  //if (joystick.begin() == false)
 // {
   // Serial.println("Joystick not connected");
  //  while (1);
//  }
}

void loop() {

  unsigned long start = millis(); // Start of sample window
  unsigned int peakToPeak = 0;   // peak-to-peak level
  unsigned int signalMax = 0;
  unsigned int signalMin = 1024;

  unsigned int peakToPeak2 = 0;   // peak-to-peak level
  unsigned int signalMax2 = 0;
  unsigned int signalMin2 = 1024;

  // collect data within sample window
  while (millis() - start < sampleWindow)
  { shout = analogRead(0);
    if (shout < 1024)  //This is the max of the 10-bit ADC so this loop will include all readings
    {
      if (shout > signalMax)
      {
        signalMax = shout;  // save just the max levels
      }
      else if (shout < signalMin)
      {
        signalMin = shout;  // save just the min levels
      }
    }
  }
  peakToPeak = signalMax - signalMin;  // max - min = peak-peak amplitude
  double volts = (peakToPeak * 3.3) / 1024;  // convert to volts




  //Joystick attack
//  Serial.println(volts);

  if (volts >= 1 && volts <= 2.49 && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
   // digitalWrite(ledPin, HIGH); //Mic troubleshoot with LED
 //  Serial.write("bap");
    Serial.write(1); //1 is light attack
    Serial.flush();
    delay (20);
  }
  else
  {
 //   digitalWrite(ledPin, LOW);
  }

  if (volts >= 2.5 && volts <= 3.29 && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
   // digitalWrite(ledPin, HIGH);
    Serial.write(11); //11 is mid attack
    Serial.flush();
    delay (20);
  }
  else
  {
//    digitalWrite(ledPin, LOW);
  }

  if (volts >= 3.30  && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
   // digitalWrite(ledPin, HIGH);
    Serial.write(111); //111 is heavy attack
    Serial.flush();
    delay (20);
  }
  else
  {
 //   digitalWrite(ledPin, LOW);
  }

  //Joystick Directional
  if (joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  //  Serial.println("idle"); 
    Serial.write(100);
    Serial.flush();
    delay (20);
  }
  
  if (joystick.getVertical() < 400) {
  //  Serial.println("Y: up ");
    Serial.write(2);
    Serial.flush();
    delay (20);
  }


  if (joystick.getVertical() == 1023) {
 // Serial.println("Y: down ");
    Serial.write(3);
    Serial.flush();
    delay (20);
  }


  if (joystick.getHorizontal() == 0 ) {
   // Serial.println("X: block ");
    Serial.write(4);
    Serial.flush();
    delay (20);
  }


  if (joystick.getHorizontal() == 1023 ) {
   // Serial.println("X: grab ");
    Serial.write(5);
    Serial.flush();
    delay (20);
  }



}
