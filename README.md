# InSightWindow Microcontroller 

The InSightWindow system incorporates microcontroller technology, specifically the ESP32, to enable seamless remote control of windows. By leveraging **SignalR**, the microcontroller can maintain real-time communication with the backend API, ensuring instant updates and efficient control.

### Microcontroller Features
- **ESP32 with nanoFramework**: The microcontroller is powered by the ESP32 running on nanoFramework, providing a powerful yet efficient platform for managing the window hardware. This setup ensures reliable performance, low power consumption, and robust connectivity via Wi-Fi.

### Integration with SignalR and API
- **Real-Time Communication**: SignalR is used to establish a persistent connection between the microcontroller and the backend API. This allows for real-time updates and commands to be sent to and from the device, enabling instantaneous window operation.
- **Bi-Directional Data Flow**: The microcontroller can send data, such as sensor readings or window status, directly to the server while simultaneously receiving control commands (e.g., open, close, adjust position) from the client application.
- **Event-Driven Architecture**: SignalR enables an event-driven architecture, where actions such as opening and closing are triggered by events from the server or user commands. This ensures highly responsive and scalable performance.
  
### Key Capabilities
- **Automatic Status Updates**: The ESP32 sends periodic status updates (e.g., window position, battery level, security status) to the server, which can be accessed via the app.
- **Efficient Power Management**: With the help of nanoFramework and event-driven communication through SignalR, the microcontroller conserves power when idle but wakes up instantly to handle incoming commands.
- **Seamless Synchronization**: SignalR ensures that the state of the windows remains synchronized across all devices, providing a consistent experience no matter where the control is issued from.
---

*This microcontroller system is under development and some features are still in progress.*

Visit our GitHub repositories for more details:  
- [InSightWindow-App][app-url]  
- [InSightWindow-Server][api]
- [InSightWidnow-Site on GitHub][site]

---

[app-url]: https://github.com/arsenpaw/InSightWindow-App  
[api]: https://github.com/arsenpaw/InSightWindow-Service
[site]: https://github.com/arsenpaw/InSightWindow-ReactSite
