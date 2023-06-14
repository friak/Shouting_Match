#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick2;


const int sampleWindow = 200; // Sample window width in milliseconds
unsigned int shout2;

void setup() {
 Serial.begin(9600);
  if (joystick2.begin() == false)
  {
    Serial.println("Joystick does not appear to be connected. Please check wiring. Freezing...");
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
  shout2 = analogRead(0);
  if (shout2 < 1024) {  //This is the max of the 10-bit ADC so this loop will include all readings
    if (shout2 > signalMax) {
      signalMax = shout2;  // save just the max levels
      }
      else if (shout2 < signalMin) {
      signalMin = shout2;  // save just the min levels
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
  Serial.write(200); //1 is light attack  
  Serial.flush();
  delay (200);
  //debug
  //Serial.println("LIGHT");
  }

 if (volts >= .61 && volts <= 1.60) {// && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  Serial.write(22); //11 is mid attack
  Serial.flush();
  delay (200);
  //debug
  //Serial.println("MED");
  }

  if (volts >= 2.00) {//  && joystick.getVertical() == 499 && joystick.getHorizontal() == 510) {
  Serial.write(222); //111 is heavy attack
  Serial.flush();
  delay (200);
  //debug
  //Serial.println("HEAVY");
  }

  //DIRECTIONAL CONTROLS
 // if (joystick2.getVertical() == 499 && joystick2.getHorizontal() == 510) {
 //   Serial.write(100);
 //   Serial.flush();
 //   delay (200);
    //debug
    //Serial.println("idle"); 
    // Serial.println(joystick.getVertical());
 // }
  
  if (joystick2.getVertical() < 400) {
    Serial.write(6);
    Serial.flush();
    delay (300);
    //debug
    //Serial.println("Y: jump ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick2.getVertical() == 1023) {
    Serial.write(7);
    Serial.flush();
    delay (300);
    //debug
    //Serial.println("Y: crouch ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick2.getHorizontal() == 0 ) {
    Serial.write(8);
    Serial.flush();
    delay (300);
    //debug
    //Serial.println("X: left ");
    // Serial.println(joystick.getVertical());
  }


  if (joystick2.getHorizontal() == 1023 ) {
    Serial.write(9);
    Serial.flush();
    delay (300);
    //debug
    //Serial.println("X: right ");
    // Serial.println(joystick.getVertical());
  }

}
