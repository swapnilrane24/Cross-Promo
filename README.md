# Cross-Promo
 Cross Promote games across other games
 
Installation:
- Import plugin to project
- Drag the canvas prefab to splash screen [1st game screen]
Assets/CrossPromo/Prefab/CrossPromoCanvas.prefab
- Set up the URL to Promo Data URL field. This url points to the JSON file.
URL example: https://www.dropbox.com/s/gm0ualys3ms3ico/CrossPromoData.json?raw=1
- Set the “Button Show Delay” time. The value is in second. The promo button will be shown
after the delay time is complete. The time calculation started at launch of game.
- Run the game, as soon as time is up the Promo Icon will be shown.
Setting up JSON data:
- Go to Windows/Devshifu/Cross Promo
- Window will popup where you can add or remove the data.
- I have uploaded the files to DropBox so the URL should end with raw=1 after the ?, else the
data will not be downloaded
- After adding the required details click on Generate File button to create a new files.
The new file will be created at path Assets/CrossPromo/PromoFiles/CrossPromoData.json
- Upload this file and now you are ready.

