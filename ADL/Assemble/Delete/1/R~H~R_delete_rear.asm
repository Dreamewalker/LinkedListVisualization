aLine 0
gNewVPtr nextPtr
gMoveNext nextPtr, Root
gBne Root, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gBne nextPtr, Root, 9

aLine 4
nDelete Root

aLine 5
gMove Root, null

aLine 6
gDelete nextPtr
aStd
Halt

aLine 8
gNew prevPtr
gMove prevPtr, Root

aLine 9
gNew delPtr
gMove delPtr, nextPtr
gMoveNext nextPtr, delPtr

aLine 10
gBeq nextPtr, Root, 7

aLine 11
gMove prevPtr, delPtr

aLine 12
gMove delPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -7

aLine 14
nMoveRelOut delPtr, delPtr, 100
pDeleteNext delPtr
pSetNext prevPtr, Root

aLine 15
nDelete delPtr

aLine 16
gDelete delPtr
gDelete prevPtr
gDelete nextPtr
aStd
Halt