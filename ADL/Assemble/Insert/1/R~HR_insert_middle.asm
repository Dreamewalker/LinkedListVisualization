aLine 0
gNew currentPtr
gMove currentPtr, Root

aLine 1
sInit i, 0
sBge i, {1:D}, 11

aLine 2
gBne currentPtr, Root, 4
sBle i, 0, 3

aLine 3
Exception NOT_FOUND

aLine 5
gMoveNext currentPtr, currentPtr

aLine 1
sInc i, 1
Jmp -10

aLine 7
gBne currentPtr, Root, 4
sBle i, 0, 3

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
gBne temp, Root, 3

aLine 13
gMove Rear, newNodePtr

aLine 15
pSetNext currentPtr, newNodePtr

aLine 16
gDelete currentPtr
gDelete temp
gDelete newNodePtr
aStd
Halt