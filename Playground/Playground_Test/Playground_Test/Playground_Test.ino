#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>
#endif

#define PIN 6

int n = 0;
boolean up = true;

char blueToothVal;           //value sent over via bluetooth
char lastValue;

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(144, PIN, NEO_GRB + NEO_KHZ800);

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

uint32_t magenta = strip.Color(255, 0, 255);
uint32_t orange = strip.Color(255, 165, 0);
uint32_t green = strip.Color(0, 255, 0);


void setup() {
	Serial.begin(9600);

	strip.begin();
	strip.setBrightness(30);
	strip.show(); // Initialize all pixels to 'off'
}

void getArrayFromSerial(int* arr, int size) {
	for (int i = 0; i < size; i++)
	{
		int read = Serial.parseInt();		
		arr[i] = read;
		Serial.println(arr[i]);
	}
}

void commandOne(int address) {
	int arr[3] = { 0 };	
	Serial.println("Getting address");
	getArrayFromSerial(arr, 3);

	Serial.println("Command One");
	Serial.print("address: ");
	Serial.println(address);


	strip.setPixelColor(address, arr[0], arr[1], arr[2]);
}

void commandTwo(int address) {
	int arr[3] = { 0 };
	Serial.println("Getting values");
	getArrayFromSerial(arr, 4);

	Serial.println("Command One");
	Serial.print("address: ");
	Serial.println(address);

	int length = arr[3];

	for (int i = address; i < address + length; i++) {
		strip.setPixelColor(i, arr[0], arr[1], arr[2]);
	}	
}


void loop() {
	
	while (Serial.available() > 0) {
		byte read = Serial.read();

		if (read != '>') {
			continue;
		}

		int command = Serial.parseInt();
		int address = Serial.parseInt();			
		
		if (command == 1) {
			commandOne(address);
			//strip.setPixelColor(address, r, g, b);
		}

		if (command == 2) {
			commandTwo(address);
			//strip.setPixelColor(address, r, g, b);
		}

		int r = Serial.read();

		if (r == '<') {
			
			strip.show();
			Serial.println("Showing strip");
		}
		else {
			Serial.println("**Not Showing strip");
			Serial.println(r);
		}
	}

	//if (Serial.available())
	//{//if there is data being recieved
	//	blueToothVal = Serial.read(); //read it
	//}
	//if (blueToothVal == 'n')
	//{//if value from bluetooth serial is n
	//	digitalWrite(13, HIGH);            //switch on LED
	//	if (lastValue != 'n')
	//		Serial.println(F("LED is on")); //print LED is on
	//	lastValue = blueToothVal;
	//}
	//else if (blueToothVal == 'f')
	//{//if value from bluetooth serial is n
	//	digitalWrite(13, LOW);             //turn off LED
	//	if (lastValue != 'f')
	//		Serial.println(F("LED is off")); //print LED is on
	//	lastValue = blueToothVal;
	//}



	/*for (int i = 30; i < 36; i++) {
		strip.setPixelColor(i, orange);
	}

	for (int i = 60; i < 72; i++) {
		strip.setPixelColor(i, orange);
	}

	for (int i = 100; i < 115; i++) {
		strip.setPixelColor(i, magenta);
	}

	for (int i = 130; i < 135; i++) {
		strip.setPixelColor(i, orange);
	}*/


	//strip.setPixelColor(25, green);

	//if (n == 144) {
	//	up = false;
	//}

	//if (n == 0) {
	//	up = true;
	//}

	//if (up) {
	//	n++;
	//}
	//else {
	//	n--;
	//}
	////Serial.println(n);
	//if (up) {
	//	strip.setPixelColor(n-1, 0, 0, 0);
	//}
	//else {
	//	strip.setPixelColor(n + 1, 0, 0, 0);
	//}
	//strip.setPixelColor(n, 0, 255, 0);

	//
	//
	//strip.show();
	//delay(10);

	// Some example procedures showing how to display to the pixels:
	//colorWipe(strip.Color(255, 0, 0), 50); // Red
	//colorWipe(strip.Color(0, 255, 0), 50); // Green
	//colorWipe(strip.Color(0, 0, 255), 50); // Blue
										   // Send a theater pixel chase in...
	//theaterChase(strip.Color(127, 127, 127), 50); // White
	//theaterChase(strip.Color(127, 0, 0), 50); // Red
	//theaterChase(strip.Color(0, 0, 127), 50); // Blue

	//rainbow(20);
	//rainbowCycle(20);
	//theaterChaseRainbow(50);

	//strip.setPixelColor(n, red, green, blue);
}

// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
	for (uint16_t i = 0; i<strip.numPixels(); i++) {
		strip.setPixelColor(i, c);
		strip.show();
		delay(wait);
	}
}

void rainbow(uint8_t wait) {
	uint16_t i, j;

	for (j = 0; j<256; j++) {
		for (i = 0; i<strip.numPixels(); i++) {
			strip.setPixelColor(i, Wheel((i + j) & 255));
		}
		strip.show();
		delay(wait);
	}
}

// Slightly different, this makes the rainbow equally distributed throughout
void rainbowCycle(uint8_t wait) {
	uint16_t i, j;

	for (j = 0; j<256 * 5; j++) { // 5 cycles of all colors on wheel
		for (i = 0; i< strip.numPixels(); i++) {
			strip.setPixelColor(i, Wheel(((i * 256 / strip.numPixels()) + j) & 255));
		}
		strip.show();
		delay(wait);
	}
}

//Theatre-style crawling lights.
void theaterChase(uint32_t c, uint8_t wait) {
	for (int j = 0; j<10; j++) {  //do 10 cycles of chasing
		for (int q = 0; q < 3; q++) {
			for (int i = 0; i < strip.numPixels(); i = i + 3) {
				strip.setPixelColor(i + q, c);    //turn every third pixel on
			}
			strip.show();

			delay(wait);

			for (int i = 0; i < strip.numPixels(); i = i + 3) {
				strip.setPixelColor(i + q, 0);        //turn every third pixel off
			}
		}
	}
}

//Theatre-style crawling lights with rainbow effect
void theaterChaseRainbow(uint8_t wait) {
	for (int j = 0; j < 256; j++) {     // cycle all 256 colors in the wheel
		for (int q = 0; q < 3; q++) {
			for (int i = 0; i < strip.numPixels(); i = i + 3) {
				strip.setPixelColor(i + q, Wheel((i + j) % 255));    //turn every third pixel on
			}
			strip.show();

			delay(wait);

			for (int i = 0; i < strip.numPixels(); i = i + 3) {
				strip.setPixelColor(i + q, 0);        //turn every third pixel off
			}
		}
	}
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
	WheelPos = 255 - WheelPos;
	if (WheelPos < 85) {
		return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
	}
	if (WheelPos < 170) {
		WheelPos -= 85;
		return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
	}
	WheelPos -= 170;
	return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
}