Multiplayer Core 
By Woyboy 
Unity 6 
♦ What is Multiplayer Core? 
MultiplayerCore is a small Unity asset built around Unity’s solution to multiplayer 
(Netcode for GameObjects, NGO. The purpose of this package is to help unity 
developers quickly prototype multiplayer games using Relay services. This 
package is easily modifiable to the developers heart and can be easily expanded 
into a bigger project. 

♦ Features - - - - - - - 

3D Humanoid Rig with a animation blend tree walking in all directions 
(Credits to elbolillodur) 
First person controller (CharacterController component) 
Stamina/Health System + Sprinting 
Spectator System 
Network Interactables System 
Network Synchronization + Relay Services setup with the Unity Transport 
Relay 
ConnectionsManager script handling Network connections based on Game 
States & others 
♦ Installation & Setup 
Installed through the GitHub repository you can download the UnityPackage 
asset by itself and import it into your project along with dependencies. This asset 
utilizes the following packages that you’re expected to have: 
Cinemachine - - - 
Netcode for GameObjects 
Multiplayer Tools (and other related workflow packages for debugging) 
After installation, everything is straight forward, you can find all the assets 
organized properly in the Resources folder and scripts organized between the 
Network and Player. 
Layers 
Additionally, your project requires specific Layers. Make sure you have included 
layers: 
+ LocalPlayerBody 
+ Assign this layer to HideFromLocalCamera property in 
NetworkFirstPersonCameraController.cs 
+ DeadPlayer 
+ No need to be assigned, it’s pre-assigned in 
NetworkFirstPersonCameraController.cs 
+ Interactable 
+ Assign this layer to the Interact Layer property in 
NetworkPlayerInteractionController.cs 
The project contains 3 scenes named ConnectionScene, Lobby, and 
MultiplayerDemo. Technically you could just use ConnectionScene and Lobby. 
But, having a Lobby and MultiplayerDemo can help organize your online match’s 
GameState enum to avoid connections mid-game.  
Each scene contains core prefabs to making a functioning multiplayer game, 
reference the scenes if you want to make a scene from scrap. Referencing it can 
be tough and time consuming so it’s recommended to just duplicate the scenes 
and change it from there. Just ensure that your NetworkManager object has a 
player prefab and a NetworkPrefabsList. Don’t forget to set the scene names 
directly in the ConnectionsManager’s editor when editing these scenes. 
The Lobby and MultiplayerDemo scene also contains interactable environments 
for the player to interact with and test out the mechanics, please go over the 
scripts and understanding the architecture behind NetworkInteractable.cs  
♦ Other Information 
How do I teleport the player? - 
You can access the player’s teleport method in the NetworkPlayer.cs in  
void TeleportTo(Transform targetPos) 
How do I damage/heal the player? - 
You can access the damage and health method  in the 
NetworkPlayerStats.cs in  void TakeDamageServerRPC(int amount). The 
same applies to health. 
How do I disconnect a player? - 
Access through ConnectionsManager.cs and find the void 
LeaveGameCient() method. 
How do I join as a client? - 
Access through ConnectionsManager.cs and find the void JoinGame() 
method. 
How do I start as a Host? - 
Access through ConnectionsManager.cs and find the async HostGame() 
method. 
How do I set GameState? - 
Access through ConnectionsManager.cs and find the void SetGameState() 
method. 
How are interactions done? - 
Take inspiration from the MultiplayerDemo scene and take a look at the 
interactable scripts. They all inherit the script, NetworkInteractable.cs 
which can be modified for specific situations as you can find in the 
examples. 
♦ Developer Note 
This is my second attempt at Unity Multiplayer’s solution (Netcode with 
GameObjects). So don’t expect too much. The only thing you may find here are the 
important essentials to quickly prototype a multiplayer co-op game, mainly aimed 
at horror but can be expanded. Happy developing! 
