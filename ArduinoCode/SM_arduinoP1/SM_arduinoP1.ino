#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick; //Create instance of this object

const int sampleWindow = 200; // Sample window width in milliseconds
unsigned int shout;

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
  unsigned int peakToPeak = 0;   // peak-to-peak level
  unsigned int signalMax = 0;
  unsigned int signalMin = 1024;

  unsigned int peakToPeak2 = 0;   // peak-to-peak level
  unsigned int signalMax2 = 0;
  unsigned int signalMin2 = 1024;

 // collect data within sample window
 while (millis() - start < sampleWindow) { 
  shout = analogRead(0);
  if (shout < 1024) { //This is the max of the 10-bit ADC so this loop will include all readings
    if (shout > signalMax) {
      signalMax = shout;  // save just the max levels
      }
    else if (shout < signalMin) {
      signalMin = shout;  // save just the min levels
     }
  }
 }
  peakToPeak = signalMax - signalMin;  // max - min = peak-peak amplitude
  double volts = (peakToPeak * 3.3) / 1024;  // convert to volts
  
  //debug
//  Serial.println(volts);
 // delay (500);


 
//ATTACK CONTROLS
if (volts >= .20 && volts <= .60) { // && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  Serial.write(1); //1 is light attack  
  Serial.flush();
  delay (200);
  //debug
//  Serial.println("LIGHT");
  }

 if (volts >= .61 && volts <= 1.60) {// && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  Serial.write(11); //11 is mid attack
  Serial.flush();
  delay (200);
  //debug
//  Serial.println("MED");
  }

  if (volts >= 2.00) {//  && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  Serial.write(111); //111 is heavy attack
  Serial.flush();
  delay (200);
  //debug
//  Serial.println("HEAVY");
  }

  //DIRECTIONAL CONTROLS
  if (joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
    Serial.write(100);
    Serial.flush();
    delay (200);
    //debug
//    Serial.println("idle"); 
    // Serial.println(joystick.getVertical());
  }
  
  if (joystick.getVertical() < 400) {
    Serial.write(2);
    Serial.flush();
    delay (300);
    //debug
//    Serial.println("Y: jump ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick.getVertical() == 1023) {
    Serial.write(3);
    Serial.flush();
    delay (300);
    //debug
    //Serial.println("Y: crouch ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick.getHorizontal() == 0 ) {
    Serial.write(4);
    Serial.flush();
    delay (300);
    //debug
   // Serial.println("X: left ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick.getHorizontal() == 1023 ) {
    Serial.write(5);
    Serial.flush();
    delay (300);
    //debug
   // Serial.println("X: right ");
    // Serial.println(joystick.getVertical());
  }



}
