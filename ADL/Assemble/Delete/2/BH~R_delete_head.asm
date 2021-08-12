aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
gBne delPtr, null, 3

aLine 2
Exception EMPTY_LIST

aLine 4
nMoveRel delPtr, delPtr, 0, -164.545
gNewVPtr delNext
gMoveNext delNext, delPtr
pSetNext Root, delNext

aLine 5
gBeq delNext, null, 3

aLine 6
pSetPrev delNext, Root

aLine 8
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr
gDelete delNext

aLine 6
aStd
Halt