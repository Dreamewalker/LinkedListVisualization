aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
sInit i, 0
sBge i, {0:D}, 10

aLine 2
gBne delPtr, null, 3

aLine 3
Exception NOT_FOUND

aLine 5
gMoveNext delPtr, delPtr

aLine 1
sInc i, 1
Jmp -9

aLine 7
gBne delPtr, null, 3

aLine 8
Exception NOT_FOUND

aLine 10
gNewVPtr delNext
gMoveNext delNext, delPtr
gNewVPtr delPrev
gMovePrev delPrev, delPtr
nMoveRel delPtr, delPtr, 0, -164.545
gBne delPtr, Rear, 4

aLine 11
gMove Rear, delPrev
Jmp 3

aLine 14
pSetPrev delNext, delPrev

aLine 16
pSetNext delPrev, delNext

aLine 17
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr
gDelete delPrev
gDelete delNext

aLine 18
aStd
Halt