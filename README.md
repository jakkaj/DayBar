# DayBar
###DayBar - the LED strip that plans your day

Using a mixture of Arduino and Desktop software to make a strip that shows you what your day looks like. Now is the green dot. 
Meetings are orange bars!

The best part is you can make it yourself! Mine cost about $150 or so.

Basically the system uses an Arduino to drive the strip, and a PC tells the Arduino which pixels to set which color via a serial USB interface. The PC app is built using Windows 10 UWP. 

At the moment the app only supports using Office 365 calendars. There are plans to add Google support later. Perhaps. 

####What you'll need

* Windows 10
* Visual Studio 2015 (for the UWP stuff)
* And Office 365 developer account. 
* Arduino Software (or Visual Micro plugin for Visual Studio)
* An Arduino. I used a nano from a [local supplier](http://tronixlabs.com/arduino/boards/nano/arduino-nano-v3-0-compatible-with-soldered-pins-australia/) for under $10. 
* A 144 pixel led strip. I used an Adafruit Neopixel strip from a [local supplier](http://tronixlabs.com/components/leds/strip/adafruit-neopixel-digital-rgb-led-strip-144-led-1m-black-australia/) (but you can probably get a cheaper non-Adafruit one if you're on a budget.)
* A 300-500 ohm resistor
* A capacitor 1000 µF, 6.3V or higher
* [5V 4amp power supply](http://tronixlabs.com/power/ac-dc/5v-dc-4a-fixed-2-1mm-tip-appliance-plugpack-australia/)
* [JST male inline power connector](http://tronixlabs.com/cables/jst/adafruit-in-line-power-wire-connector-male-australia/)
* [DC female barrel jack](http://tronixlabs.com/power/accessories/dc-barrel-jack-adapter-female-australia/)
* A solderless breadboard to prototype with
* Some jumpers / dupont connectors


You don't have to use the exact copy of these parts - get them from your preferred supplier. Tronix Labs in Sydney (near me) have been fantastic to deal with. 

You can use a smaller led pixel strip - but you'll have to modify to code to support that youself!

###Before you start!!
Make sure you read the [Adafruit Neopixel Uber Guide] (https://learn.adafruit.com/adafruit-neopixel-uberguide/overview) - especially the [Best practices](https://learn.adafruit.com/adafruit-neopixel-uberguide/best-practices) section - it might save you from killing your strip!

![Wiring Image](https://raw.githubusercontent.com/jakkaj/DayBar/master/Assets/daybar_4.JPG)

####Wiring the strip
We'll start with getting some power to the strip. The led strip draws too much current for the Arduino to power it, so we have to power the leds using an external power supply. 

* Connect the power leads (the big red and black ones) to the barrel jack along with the capacitor and screw in to place. The cap helps protect the strip from spikes when powering on. It's super sensitive!
* Pop your Arduino on the bread board (or not, if you're on a mega or uno etc). 
* Using your make JST connector, pop the negative lead to negative on the Arduino
* Pop the positivate lead of the JST via the resistor to pin 6 on the arduino. 
* In the picture above, the negative jumper cable is red - naughty!

That's it!

#### Installing the software on the Arduino

You'll need to download the code from this GitHub project, and install it on the Arduino. 

You can use [Arduino software](https://www.arduino.cc/en/Main/Software) or you can use [Visual Micro](http://www.visualmicro.com/) (you'll still need Arduino tools installed). 

The source file you need is [Playground_Test.ino](https://github.com/jakkaj/DayBar/blob/master/Playground/Playground_Test/Playground_Test/Playground_Test.ino). Very poorly named from when this project started as a Sunday coding sesh. 

##### Install Arduino Libraries
You will need to add two libraries to your project.
* [Adafruit Neopixel] (https://github.com/adafruit/Adafruit_NeoPixel)
* [Serial Command] (https://github.com/kroimon/Arduino-SerialCommand)

If you don't know how to install libraries, Google it :P. Basically, grab the code and Zip the parent folder of each thing... then use add library in the menus. 

Once you do that you should be able to deploy to your Adruino. If deployment is success then the strip should do a single Cylon/Knight Rider red blip thingy. 

