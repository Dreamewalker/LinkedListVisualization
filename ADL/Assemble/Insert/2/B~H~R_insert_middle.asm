aLine 0
gNew currentPtr
gMove currentPtr, Root

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
gBne currentPtr, null, 3

aLine 8
Exception NOT_FOUND

aLine 10
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, currentPtr

aLine 11
nMoveRel newNodePtr, currentPtr, 95, -164.545 
pSetNext newNodePtr, temp

aLine 12
gBeq temp, null, 3

aLine 13
pSetPrev temp, newNodePtr

aLine 15
pSetNext currentPtr, newNodePtr

aLine 16
pSetPrev newNodePtr, currentPtr

aLine 17
gDelete currentPtr
gDelete temp
gDelete newNodePtr
aStd
Halt