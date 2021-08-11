aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
gNew prevPtr
gMove prevPtr, Root

aLine 2
sInit i, 0
sBge i, {0:D}, 12

aLine 3
gBne delPtr, Root, 3

aLine 4
Exception NOT_FOUND

aLine 6
gMove prevPtr, delPtr

aLine 7
gMoveNext delPtr, delPtr

aLine 2
sInc i, 1
Jmp -11

aLine 9
gBne delPtr, Root, 3

aLine 10
Exception NOT_FOUND

aLine 12
gBne delPtr, Rear, 3

aLine 13
gMove Rear, prevPtr

aLine 15
gNewVPtr delNext
gMoveNext delNext, delPtr
nMoveRelOut delPtr, delPtr, 100
pSetNext prevPtr, delNext

aLine 16
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete prevPtr
gDelete delNext

aLine 17
aStd
Halt