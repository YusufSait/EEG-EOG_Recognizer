# EEG-EOG_Recognizer

The program analyzes real-time EEG/EOG data and detect actions based on the similarity between the current signal and the recorded signal pattern.

The real-time EEG/EOG data is acquired from a "ThinkGear Brain Kit" device connected via Bluetooth to the PC. Normalized cross-correlation method is implemented to determine the similarity between the current signal and previously recorded signal pattern. A dynamic-sized circular buffer data structure is utilized to efficiently record and analyze the incoming data.

The program also provides real-time visualization of the incoming data and resulting normalized cross-correlation scores.

![alt text](https://github.com/YusufSait/EEG-EOG_Recognizer/blob/main/Signal%20Similarity%20app%20UML.png?raw=true)
