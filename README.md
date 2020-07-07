# anino_exam
Unity 2019.2.14f1

ReelController:
Unfortunately handles everything from:
1. Button events
2. initiating the spinning of each reel
3. checking for winning combinations
4. Contains the 5 Reel Objects

Reel Object:
1. Contains symbols
2. Handles "rotation" of itself

SymbolsDataHolder:
1. A singleton that holds symbols from a scriptable object

Resources Folder:
Contains Data for Payout Lines and Symbols
PayLinesData can be edited
SymbolsData is still tightly coupled. I only recommend editing the PayOut field

This can handle increasing the symbols but is still fragile.
Payout lines, flexible enough to add or remove lines.

A lot of improvements with UI and separation of concerns.
