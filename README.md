# LinkedListVisualization
2020-2021学年春季学期暑假 数据结构课设1——链表可视化（基于WPF）

## 动画伪指令
### 通用指针操作
gNew newPtr   // 新建一个指针newPtr  
gMove dstPtr, srcPtr   // 将指针dstPtr指向srcPtr指向的结点  
gDelete dstPtr    // 删除dstPtr  
gNewVPtr newPtr     // 新建一个不可见的指针newPtr
gMoveNext dstPtr, srcPtr    // 将dstPtr指向srcPtr指向的结点的后继  
gMovePrev dstPtr, srcPtr    // 将dstPtr指向srcPtr指向的结点的前驱  

### 前驱/后继指针操作
pSetNext dstPtr, srcPtr  // 将指针dstPtr指向的结点的后继指针指向srcPtr指向的结点  
pSetPrev dstPtr, srcPtr  // 将指针dstPtr指向的结点的前驱指针指向srcPtr指向的结点  
pDeleteNext dstPtr  // 删除指针dstPtr指向的结点的后继指针  
pDeletePrev dstPtr  // 删除指针dstPtr指向的结点的前驱指针  
### 结点操作
nNew newPtr, value    // 新建值为value的结点，由newPtr指向  
nSetValue dstPtr, value // 将dstPtr指向的结点值设置为value
nDelete srcPtr    // 删除srcPtr指向的结点和srcPtr  
nMoveAbs dstPtr, CanvasLeft, CanvasTop  // 将dstPtr指向的结点移动至绝对位置(CanvasLeft, CanvasTop)  
nMoveRel dstPtr, CanvasLeft, CanvasTop  // 将dstPtr指向的结点移动至相对位置(CanvasLeft, CanvasTop)  

### 串动画操作
aLeft srcPtr, offset    // 将srcPtr指向的结点及其后继向左移动  
aStd     // 将整个链表移动到标准位置  

### 标量操作
sSetTime value  // 设置循环轮次  
### 程序控制
pBeq ptr1, ptr2, label  // 当ptr1与ptr2指向同一个结点时跳转到PC = label  
pBne ptr1, ptr2, label  // 当ptr1与ptr2指向不同结点时跳转到PC = label 
sBeq srcPtr, value, label   // 当srcPtr指向的结点值与value相同时跳转到PC = label  
sBne srcPtr, value, label   // 当srcPtr指向的结点值与value不同时跳转到PC = label  

sLoop label     // 若循环变量大于0，递减循环变量并跳转至label；否则继续执行  

psBge ptr1, ptr2, label  // 当ptr1指向的结点值大于等于ptr2指向的结点值时跳转到PC = label  
psBle ptr1, ptr2, label  // 当ptr1指向的结点值小于等于ptr2指向的结点值时跳转到PC = label  
Halt        // 程序结束  
Exception   // 结果异常