aLine 0
gNewVPtr nextPtr
gMoveNext nextPtr, Root
gBne Root, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gBne nextPtr, null, 9

aLine 4
nDelete Root

aLine 5
gMove Root, null

aLine 6
gDelete nextPtr
aStd
Halt

aLine 8
gNew delPtr
gMove delPtr, nextPtr
gMoveNext nextPtr, delPtr

aLine 9
gBeq nextPtr, null, 5

aLine 10
gMove delPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -5

aLine 12
gNewVPtr delPrev
gMovePrev delPrev, delPtr
pDeleteNext delPrev

aLine 13
pDeletePrev delPtr
nDelete delPtr

aLine 14
gDelete delPtr
gDelete delPrev
gDelete nextPtr
aStd
Halt