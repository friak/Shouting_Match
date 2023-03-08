#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick2;


const int sampleWindow = 150; // Sample window width in milliseconds
unsigned int shout2;
int ledPin2 = 6;

void setup() {
  Serial.begin(9600);
  pinMode(ledPin2, OUTPUT);


  if (joystick2.begin() == false)
  {
    Serial.println("Joystick does not appear to be connected. Please check wiring. Freezing...");
    while (1);
  }
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
  { shout2 = analogRead(0);
    if (shout2 < 1024)  //This is the max of the 10-bit ADC so this loop will include all readings
    {
      if (shout2 > signalMax)
      {
        signalMax = shout2;  // save just the max levels
      }
      else if (shout2 < signalMin)
      {
        signalMin = shout2;  // save just the min levels
      }
    }
  }
  peakToPeak = signalMax - signalMin;  // max - min = peak-peak amplitude
  double volts = (peakToPeak * 3.3) / 1024;  // convert to volts


 //Joystick attack
//  Serial.println(volts);

  if (volts >= 1 && volts <= 2.49 && joystick2.getVertical() == 496 && joystick2.getHorizontal() == 512) {
   // digitalWrite(ledPin2, HIGH); //Mic troubleshoot with LED
 //  Serial.write("bap");
    Serial.write(1); //1 is light attack
    Serial.flush();
    delay (20);
  }
  else
  {
  //  digitalWrite(ledPin2, LOW);
  }

  if (volts >= 2.5 && volts <= 3.29 && joystick2.getVertical() == 496 && joystick2.getHorizontal() == 512) {
   // digitalWrite(ledPin2, HIGH);
    Serial.write(11); //11 is mid attack
    Serial.flush();
    delay (20);
  }
  else
  {
  //  digitalWrite(ledPin2, LOW);
  }

  if (volts >= 3.30  && joystick2.getVertical() == 496 && joystick2.getHorizontal() == 512) {
   // digitalWrite(ledPin2, HIGH);
    Serial.write(111); //111 is heavy attack
    Serial.flush();
    delay (20);
  }
  else
  {
  //  digitalWrite(ledPin2, LOW);
  }

  //Joystick Directional
  if (joystick2.getVertical() == 496 && joystick2.getHorizontal() == 512) {
  //  Serial.println("idle"); 
    Serial.write(100); //111 is heavy attack
    Serial.flush();
    delay (20);
  }
  
  if (joystick2.getVertical() < 400) {
  //  Serial.println("Y: up ");
    Serial.write(2);
    Serial.flush();
    delay (20);
  }


  if (joystick2.getVertical() == 1023) {
 // Serial.println("Y: down ");
    Serial.write(3);
    Serial.flush();
    delay (20);
  }


  if (joystick2.getHorizontal() == 0 ) {
   // Serial.println("X: grab ");
    Serial.write(4);
    Serial.flush();
    delay (20);
  }


  if (joystick2.getHorizontal() == 1023 ) {
   // Serial.println("X: block/grab ");
    Serial.write(5);
    Serial.flush();
    delay (20);
  }
}
