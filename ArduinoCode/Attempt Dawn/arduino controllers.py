from pyfirmata import Arduino, util
from pyfirmata.util import Iterator
import time
from math import floor
from vosk import Model, KaldiRecognizer
import pyaudio
model = Model(r"C:\Users\Khan\Documents\GitHub\Shouting_Match\ArduinoCode\Attempt Dawn\test en")
recognizer = KaldiRecognizer(model,16000)
capt= pyaudio.PyAudio()
stream = capt.open(format=pyaudio.paInt16, channels=1, rate=16000, input=True, frames_per_buffer=8192)
stream.start_stream()

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

joystick_x = board.get_pin("a:4:i")
joystick_y = board.get_pin("a:5:i")
joystick_s = board.get_pin("a:2:i")
joystick_shout = board.get_pin("a:0:i")
joystick_x.enable_reporting()
joystick_y.enable_reporting()
joystick_s.enable_reporting()
joystick_shout.enable_reporting()

dt = .1

#val_x, val_y, val_s = .5, .5, 0



while True:
    time.sleep(dt*5)
    val_x = joystick_x.read()
    val_y = joystick_y.read()
    val_s = joystick_s.read()
    val_shout = joystick_shout.read()
    print('X',val_x, 'Y', val_y, 'Button',val_s)
    #data = stream.read(9600)
    #if recognizer.AcceptWaveform(data):
        #print(recognizer.Result())
    #val_s = floor(joystick_s.read())
    # if val_x is None or val_y is None or val_s:
    #     val_x, val_y, val_s = .5, .5, 0
    print('X',val_x, 'Y', val_y, 'Button',val_s,'shout',val_shout)
   
