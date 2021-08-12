aLine 0
gNew delPtr
gMove delPtr, Root

aLine 1
gBne Root, null, 3

aLine 2
Exception EMPTY_LIST

aLine 4
gMoveNext Root, Root

aLine 5
pDeletePrev Root

aLine 6
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr

aLine 7
aStd
Halt