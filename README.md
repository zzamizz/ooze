# ooze
Savefile editor & viewer for the **PS2** release of **Monsters, Inc. Scare Island**
![v1.00](/resources/ooze.png)

## Features
- Edit lives
- Edit all ooze counts
- Set flags for *specific* nerves or tokens in a map
- Settings (volumes, rumble, stereo) and a flag for the ingame Cheats menu
- 100% preset button: sets everything to 100 and checks all boxes

## Usage
Current version (1.00) only supports PCSX2's folder-type memory cards.
1. Run ooze
2. Find your PCSX2's `\memcards` directory, and then your memory card folder.
3. Choose Scare Island's savefile folder. The name is one of the following:
- BESCES-50595
- BESCES-50596
- BESCES-50597
- BESCES-50598
- BESCES-50599
- BESCES-50600
- BESCES-50601
- BESCES-50602
- BESCES-50603
- BESCES-50604
- BESCES-50605
4. Inside the folder there are files named after one of the above, followed by 001, 002, etc. These correspond to your 4 save slots ingame. Select one of them.
5. Start editing.
### Notes
- Make sure PCSX2 is not currently using the memory card.
- You can hover on most checkboxes to read more information.
- Make sure to press "Save" at the end.
## Credit
- fr4nk0: for staring and fixing some array problems for 5 hours straight / being a "project supervisor"
## To-do
- Add support for all common memory card types
- Document some unknown bitfields and perhaps add them into the program
- Can you unlock movies?
- Find how trampolines, speed arrows and slingshots are unlocked then add them.
- There are separately uint32 versions of the nerve & token counts, editing only the bitfields does not affect them at the moment. This could cause funky glitches at times where the game uses current nerve count to unlock something (a couple cases)
- Fix fucked up checkboxes (squished box = checkmark too big to display properly)
- Add scale toggles for WPF (window scale is definitely goofy)