aLine 0
gNewVPtr nextPtr
gMoveNext nextPtr, Root
gBne nextPtr, Root, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew prevPtr
gMove prevPtr, Root

aLine 4
gNew delPtr
gMove delPtr, nextPtr
gMoveNext nextPtr, delPtr

aLine 5
gBeq nextPtr, Root, 7

aLine 6
gMove prevPtr, delPtr

aLine 7
gMove delPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -7

aLine 9
nMoveRelOut delPtr, delPtr, 100
pDeleteNext delPtr
pSetNext prevPtr, Root

aLine 10
nDelete delPtr

aLine 11
gDelete delPtr
gDelete prevPtr
gDelete nextPtr
aStd
Halt