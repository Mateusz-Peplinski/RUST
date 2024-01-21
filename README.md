<p align="center">
  <img src="icon/RUST_icon.png" alt="Logo" width="150" height="150">
</p>

<h1 align="center">🔧 RUST</h1>

<p align="center">
  RUST is a general-purpose computer tool coded in C# with the .NET Framework (4.7.2) and win32 API.
</p>

<p align="center">
  <img src="READMEimg/Capture.PNG" alt="Application.exe">
</p>

RUST is a command-line interface tool where the user mainly navigates using numbers and the ↵ Enter key.

The binaries (executable `.exe`) can be found in 📁 `RUST/binaries/RUST.exe`

## 📜 Main Menu:
<p align="center">
  <img src="READMEimg/Capture1.PNG" alt="AppMenu">
</p>

## ✨ Features:
Below are some of the notable features of RUST, with brief screenshots provided:

- **CPU/RAM Stats and Details**
  - Live CPU load Bars:
    - ![CPUBars](READMEimg/Capture2.PNG)
  - CPU Info:
    - ![CPUInfo](READMEimg/Capture3.PNG)
  - RAM Info:
    - ![RAMInfo](READMEimg/Capture4.PNG)

- **Process Management**
  - View and interact with running processes:
    - 🗂️ Select a process using the PID and automatically create a dump (`.dmp`) from memory.
      - ![Show Processes](READMEimg/Capture5.PNG)
    - 💾 Dump Process:
      - ![Dump Process](READMEimg/Capture6.PNG)
    - 🔪 Select a process using its PID and kill it.

- **Network Scanner**
  - 🎯 Enter a target IP address and automatically try to ping it.
    - ![Ping IP](READMEimg/Capture7.PNG)
  - 🚪 Enter a target IP address and scan which ports are open.
    - ![Scan Ports](READMEimg/Capture8.PNG)
  - 🌐 Enter a Network IP address (with subnet mask 255.255.255.0) and perform a subnet scan of client IPs from `.2` -> `.255` to see which are responsive.
    - ![Subnet Scan](READMEimg/Capture9.PNG)

## 🛠️ Building:
- Written using Visual Studio 2019 Enterprise Edition.
- Compiled with the C# compiler and .NET Framework (4.7.2) to produce `RUST.exe`.

## ℹ️ About:
- **Author**: Mateusz Peplinski
- **Version 1.0 Published**: 01/08/2021
- **License**: MIT License

© 2021 Mateusz Peplinski. All rights reserved.
