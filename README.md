# DoorsAR

## Presentation

This project has been realized for a VR course at [Polytech Paris-Sud](https://www.polytech.u-psud.fr).

It allows students or personals to get the schedule of a room thanks to a QR Code placed on the door. 

## How it works

This projects contains to sub-projects:
- A NodeJS script used to make a REST API providing the paginated schedule. When querying the schedule for a specific room, it retrieves the information for Polytech's scheduler (ADE) in an iCal format, then orders, paginates and transform it in a JSON format.
- A Unity project made with Vuforia. The result is an Android app using the camera to track the target and the QR Code.

## What it needs

### To run the API (one is hosted, optional) :
- NodeJS with NPM

### To run and build the app
- Android SDK for your phone
- Unity (strongly recommended: 2017.4.2f2, available [here](https://unity3d.com/fr/get-unity/download/archive))
- An Android phone - if you want to test it on a device

## How to run

### To run the API
Go in the `RESTServer` folder.
Run the following commands: 
```
npm install
node icalParser.js
```

You can test the API by navigating to [http://localhost:1234/info/1396](http://localhost:1234/info/1396).

### To build the app
Using unity, import the project.

It already contains Vuforia's development licence. Ensure your Android SDK is installed and that your phone has the `USB debugging` on.

_If you want to use your API_ : open `ScheduleRetriever.cs` and change the API_URL parameter to your own URL (localhost or hosted).

Go to `file` and select `Build & Run`. That will generate the APK file and upload it to your phone.

