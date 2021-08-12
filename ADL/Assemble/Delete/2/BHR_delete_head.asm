aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
gBne delPtr, null, 3

aLine 2
Exception EMPTY_LIST

aLine 4
gBne delPtr, Rear, 3

aLine 5
gMove Rear, Root

aLine 7
nMoveRel delPtr, delPtr, 0, -164.545
gNewVPtr delNext
gMoveNext delNext, delPtr
pSetNext Root, delNext

aLine 8
gBeq delNext, null, 3

aLine 9
pSetPrev delNext, Root

aLine 11
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr
gDelete delNext

aLine 12
aStd
Halt