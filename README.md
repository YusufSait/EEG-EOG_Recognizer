# EEG-EOG Recognizer

C# Windows Forms project for analyzing real-time EEG/EOG data, which detects actions by comparing the current signal with stored signal patterns.

https://github.com/user-attachments/assets/c2d861eb-4ee6-44b8-9523-c99c7bca4660

The real-time EEG/EOG data is acquired from a "ThinkGear Brain Kit" device connected via Bluetooth to the PC. Normalized cross-correlation method is implemented to determine the similarity between the current signal and previously recorded signal pattern. A dynamic-sized circular buffer data structure is utilized to efficiently record and analyze the incoming data.

The program also provides real-time visualization of the incoming data and resulting normalized cross-correlation scores.

The program is implemented using the observer pattern.

![alt text](https://github.com/YusufSait/EEG-EOG_Recognizer/blob/main/Signal%20Similarity%20app%20UML.png?raw=true)

This project was completed as a student project at Gediz University - Computer Engineering Department in 2014.
