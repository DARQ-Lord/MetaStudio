# MetaStudio - Metaverse Platform for Artists to Create AI ART
## Overview
MetaStudio is a revolutionary metaverse platform designed to empower artists with the capabilities to create AI-powered art. By merging the world of art and artificial intelligence, MetaStudio provides an immersive and collaborative environment for artists to explore new frontiers of creativity. 

## Description
MetaStudio offers a comprehensive suite of AI-driven art creation tools, allowing artists to experiment with cutting-edge technologies like XR, Generative AI and Web3. These tools provide artists with the ability to transform their ideas into unique and mesmerizing AI-generated artworks.

## Vision
Our vision for MetaStudio is to democratize AI art creation and revolutionize the way artists interact with artificial intelligence. By offering an accessible and intuitive platform, we aim to bridge the gap between traditional artistry and technological innovation, empowering artists of all backgrounds to explore the endless possibilities of AI-generated art. MetaStudio envisions a world where artists can redefine artistic expression and immerse themselves in a collaborative metaverse that inspires and fosters artistic growth.

## How to Run Locally   
### Requirements
- Python 3.9
- Unity 2020.3
- Visual Studio

### Server
- Copy the contents of the Server folder into your local drive. 
- Run the following command
- ``` pip install -r requirments.txt```
- Open the path ``` Server/MetaStudio/static/BrownieProject/``` and compile the project using the command
- ```brownie compile```
- Create a .env file and add the following and replace KEY with your key.
``` export WEB3_INFURA_PROJECT_ID=KEY```
- Navigate back to Server folder. Edit you local network IP in the app.py file.
- Open the __init__.py file and enter the Private key.
- run the following command to start the server
- ```python app.py```

### Application
- Open the application in Unity
- Edit the ```NFTCreator.cs```, ```LoginManager.cs``` and ```CollectionCreator.cs``` by entering the updated URL / IP Address.
- Compile the application and run on a Meta Quest 2.


## Demo:
- Smart Contract:
  -  Code: [MetaStudio.sol](https://github.com/DARQ-Lord/MetaStudio/raw/main/Server/MetaStudio/static/BrownieProject/contracts/MetaStudioNFT.sol)
- Metaverse Application: [Metaverse App](https://github.com/DARQ-Lord/MetaStudio/raw/main/MetaStudio/FinalBuild.apk)
- Video : 

[![MetaStudio Demo | Avalanche Track |  Unfold Hackathon](http://img.youtube.com/vi/ua_YqPzQsdw/0.jpg)](https://youtu.be/ua_YqPzQsdw "MetaStudio Demo | Avalanche Track |  Unfold Hackathon")