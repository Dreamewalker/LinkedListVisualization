aLine 0
gNew currentPtr
gMoveNext currentPtr, Root

aLine 1
sInit i, 0
sBge i, {1:D}, 10

aLine 2
gBne currentPtr, null, 3

aLine 3
Exception NOT_FOUND

aLine 5
gMoveNext currentPtr, currentPtr

aLine 1
sInc i, 1
Jmp -9

aLine 7
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, currentPtr

aLine 8
nMoveRel newNodePtr, currentPtr, 95, -164.545 
pSetNext newNodePtr, temp

aLine 9
gBne temp, null, 3

aLine 10
gMove Rear, newNodePtr

aLine 12
pSetNext currentPtr, newNodePtr

aLine 13
gDelete currentPtr
gDelete temp
gDelete newNodePtr
aStd
Halt