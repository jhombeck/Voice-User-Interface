# Voice User Interfaces for Effortless Navigation in Medical Virtual Reality Environments

This project demonstrates how to use voice commands to interact with and navigate aneurysm data. 

![image](https://github.com/user-attachments/assets/2026ee75-06ae-40de-999f-9f2e2cfb092d)

The project is optimized for use with the Valve Index for a complete experience. However, a debug version with limited features is available, allowing execution without a Head-Mounted Display (HMD).

---

## Installation Guide for Windows 11

### Step 1: Download the Required Repositories
- Clone the Git repository from GitHub — [Voice-User-Interface](https://github.com/jhombeck/Voice-User-Interface)
- Download the Docker repository from Zenodo — [Zenodo Repository](https://zenodo.org/doi/10.5281/zenodo.13374003)

### Step 2: Download and Install Unity Hub
- Download Unity Hub from [Unity Hub](https://unity.com/unity-hub)
  - You will need a Unity account to log in.
- Once logged in, navigate to the **Projects** tab and click **Add**.
- Select the **Unity** folder from the cloned Git repository.
- Unity will prompt you to install the correct version for this project (Unity 2019.4.40f1).
- Install the specified Unity version.
  - **Note:** Installing Visual Studio is optional and only required if you plan to edit code.
- After the installation is complete, open the project through Unity Hub.
- Within the project, navigate to the **Scenes** folder and double-click on **Navigation_Demo** to load the scene displayed in the screenshot above.

### Step 3: Install Docker
- Download and install Docker Desktop — [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Unzip the downloaded Docker repository.
- Open a command prompt and navigate to the **.../src** directory within the unzipped Docker repository and run:


  ```bash docker-compose up --build ```
  
  - This process may take some time as all necessary components are installed.

---

## Running the Application with Voice Control

### Step 1: Set Up Docker
- Ensure Docker is running as described in the installation section.

### Step 2: Open the Unity Project
- Open the **Navigation_Demo** scene in Unity, following the instructions in the installation section.

### Step 3: Activate Voice Control
- In Unity, press **Play** in the Unity Editor.
- For users with a Valve Index HMD:
  - Press the left-hand trigger to activate the microphone.
- For users without an HMD (debug version):
  - Hold down the **Spacebar** to activate the microphone.

### Step 4: Issue Voice Commands
- While the microphone is activated, issue verbal commands to the system. For example:
  - "Rotate the object 90° around the X-axis."
- Depending on the specifications of your system, commands may take 2 to 10 seconds to execute.

---

# BibTeX Citation

If you use our project in a scientific publication please use the following citation:
