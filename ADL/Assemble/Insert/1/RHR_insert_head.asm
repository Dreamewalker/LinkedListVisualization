aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, Root

aLine 1
gBne temp, null, 7

aLine 2
gMove Rear, newNodePtr

aLine 3
nMoveRel newNodePtr, Root, 95, 164.545
pSetNext newNodePtr, Root
Jmp 4

aLine 6
nMoveRelOut newNodePtr, Root, 190
pSetNext newNodePtr, temp

aLine 8
pSetNext Root, newNodePtr

aLine 9
aStd
gDelete newNodePtr
gDelete temp
Halt