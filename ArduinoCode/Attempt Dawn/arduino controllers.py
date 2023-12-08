from pyfirmata import Arduino, util
from pyfirmata.util import Iterator
#import qwiic_joystick
import time
from math import floor

try:
    board = Arduino('COM3')
    print("here")
    it = Iterator(board)
    it.start()
    print("Successfully Connected to Arduino Board")
except:
    print("ERROR: Could Not Connect to Arduino Board")
    board = None
    exit()
#myJoystick = qwiic_joystick.QwiicJoystick()
joystick_x = board.get_pin("a:4:i")
joystick_y = board.get_pin("a:5:i")
joystick_x.enable_reporting()
joystick_y.enable_reporting()

dt = 0.1

#val_x, val_y, val_s = .5, .5, 0

#joystick_switch.write(1)

while True:
    time.sleep(dt)
    val_x = joystick_x.read()
    val_y = joystick_y.read()
    #val_s = floor(joystick_switch.read())
    # if val_x is None or val_y is None or val_s:
    #     val_x, val_y, val_s = .5, .5, 0
    print('X',val_x, 'Y', val_y)
   
