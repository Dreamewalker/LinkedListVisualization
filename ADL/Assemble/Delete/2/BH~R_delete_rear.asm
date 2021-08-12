aLine 0
gNewVPtr nextPtr
gMoveNext nextPtr, Root
gBne nextPtr, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew delPtr
gMoveNext delPtr, Root
gMoveNext nextPtr, delPtr

aLine 4
gBeq nextPtr, null, 5

aLine 5
gMove delPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -5

aLine 7
gNewVPtr delPrev
gMovePrev delPrev, delPtr
pDeleteNext delPrev

aLine 8
pDeletePrev delPtr
nDelete delPtr

aLine 9
gDelete delPtr
gDelete delPrev
gDelete nextPtr
aStd
Halt