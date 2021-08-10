aLine 0
gNew currentPtr
gMoveNext currentPtr, Root

aLine 1
sInit i, 0
sBge i, {1:D}, 10

aLine 2
gBne currentPtr, Root, 3

aLine 3
Exception NOT_FOUND

aLine 5
gMoveNext currentPtr, currentPtr

aLine 1
sInc i, 1
Jmp -9

aLine 7
gBne currentPtr, Root, 3

aLine 8
Exception NOT_FOUND

aLine 10
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, currentPtr

aLine 11
nMoveRelOut newNodePtr, currentPtr, 190
pSetNext newNodePtr, temp

aLine 12
pSetNext currentPtr, newNodePtr

aLine 13
gDelete currentPtr
gDelete temp
gDelete newNodePtr
aStd
Halt