#include <Wire.h>
#include "SparkFun_Qwiic_Joystick_Arduino_Library.h" //Click here to get the library: http://librarymanager/All#SparkFun_joystick
JOYSTICK joystick; //Create instance of this object

const int sampleWindow = 500; // Sample window width in milliseconds
int shoutInput;
// the 3 value we want to send to the computer - shout delta from the mic, x and y movement from the joystick
int shout = 0;
int verticalMove = 0;
int horizontalMove = 0;

void setup() {
  Serial.begin(9600);
  while (!Serial) {}  // wait for serial port to connect - for native USB port only}
  if (joystick.begin() == false)
    {
      Serial.println("Joystick not connected");
      while (1);
    }
  //Contact();  // send a byte until receiver responds (handshake)
}

void loop() {
  // only send if requested
  if (Serial.available() > 0) {
    if(Serial.readString() == "R"){
      
      
    }
  }

      shout = getDeltaFromMic();
      verticalMove = getVertical();
      horizontalMove = getHorizontal();

      Serial.print(shout);
      Serial.println();
      delay(30);
      Serial.print(verticalMove);
      Serial.println();
      delay(30);
      Serial.print(horizontalMove);
      Serial.println();
      delay(30);
}

void Contact(){
    while (Serial.available() <= 0) {
      Serial.write(255);   // send the largest possible byte
      delay(300);
    }
}

int getDeltaFromMic(){
    unsigned long start = millis(); // Start of sample window
    int signalMax = 0;
    int signalMin = 1024;

    // collect data within sample window
    while (millis() - start < sampleWindow) { 
      shoutInput = analogRead(0);
      signalMin = min(signalMin, shoutInput);
      signalMax = max(signalMax, shoutInput);
    }
    return signalMax - signalMin;  // max - min = peak-peak amplitude
}

int getHorizontal(){
    int value = 0;
    int move = 0;
    value = joystick.getHorizontal();
    if(value < 400){
      move = -1;
    } else if(value > 600){
      move = 1;
    }
    return move;
}

int getVertical(){
    int value = 0;
    int move = 2;
    value = joystick.getVertical();
    if(value == 0){
      move = 3;
    } else if(value == 1023) {
      move = 4;
    }
    return move;
}

