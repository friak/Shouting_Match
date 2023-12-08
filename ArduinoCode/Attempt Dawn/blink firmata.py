from pyfirmata import Arduino,util
import time
from time import sleep


board = Arduino('COM3')
print("Communication Successfully started")
loopTimes = input ('How Many times would you like the LEF to blink: ')
print ("blinking" + loopTimes + "times")
    
for x in range(int(loopTimes)):
        board.digital[13].write(1)
        time.sleep(0.2)
        board.digital[13].write(0)
        time.sleep(0.2)
