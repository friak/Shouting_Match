#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick; //Create instance of this object

const int sampleWindow = 500; // Sample window width in milliseconds
int shout;

void setup() {
 Serial.begin(9600);
  if (joystick.begin() == false)
  {
    Serial.println("Joystick not connected");
    while (1);
  }
}

void loop() {
  // MICROPHONE CODE
  unsigned long start = millis(); // Start of sample window
  unsigned int delta = 0;   // peak-to-peak level
  int signalMax = 0;
  int signalMin = 1024;

  // collect data within sample window
  while (millis() - start < sampleWindow) { 
    shout = analogRead(0);
    signalMin = min(signalMin, shout);
    signalMax = max(signalMax, shout);
  }
  delta = signalMax - signalMin;  // max - min = peak-peak amplitude
  
  if(delta > 100){
    Serial.write(delta);
    Serial.flush();
    delay (100);
    Serial.println(delta);
  } 
  else{
    Serial.write(0);
    Serial.flush();
    delay (100);
  }

  if (joystick.getVertical() > 600 ) {
      Serial.write(1);
      Serial.flush();
      delay (30);
      //debug
    Serial.println("CROUCH"); // this is going to be block for now (we hav no crouch sprite)
      // Serial.println(joystick.getVertical());
  }
  
  if(joystick.getVertical() >= 450 && joystick.getVertical() <= 600 ){
      Serial.write(2);
      Serial.flush();
      delay (30);
      //debug
    Serial.println("NO CROUCH OR JUMP");
  }
  
  if (joystick.getVertical() < 450 ) {
      Serial.write(3);
      Serial.flush();
      delay (30);
      //debug
    Serial.println("JUMP");
      // Serial.println(joystick.getVertical());
    }


  if (joystick.getHorizontal() > 600 ) {
      Serial.write(4);
      Serial.flush();
      delay (30);
      //debug
      Serial.println("RIGHT");
      // Serial.println(joystick.getVertical());
    } 
    
    if(joystick.getHorizontal() >= 450 && joystick.getHorizontal() <= 600 )
    {
      Serial.write(5);
      Serial.flush();
      delay (30);
      //debug
      Serial.println("IDLE");
    }
    
    if (joystick.getHorizontal() < 450 ) {
      Serial.write(6);
      Serial.flush();
      delay (30);
      //debug
      Serial.println("LEFT");
      // Serial.println(joystick.getVertical());
    }
}
