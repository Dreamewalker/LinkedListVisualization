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
gBeq delNext, null, 3

aLine 11
pSetPrev delNext, delPrev

aLine 13
pSetNext delPrev, delNext

aLine 14
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr
gDelete delPrev
gDelete delNext

aLine 15
aStd
Halt