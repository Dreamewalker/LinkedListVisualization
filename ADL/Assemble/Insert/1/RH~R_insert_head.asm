aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, Root

aLine 1
gBne temp, null, 5

aLine 2
nMoveRel newNodePtr, Root, 95, 164.545
pSetNext newNodePtr, Root
Jmp 4

aLine 5
nMoveRelOut newNodePtr, Root, 190
pSetNext newNodePtr, temp

aLine 7
pSetNext Root, newNodePtr

aLine 8
aStd
gDelete newNodePtr
gDelete temp
Halt