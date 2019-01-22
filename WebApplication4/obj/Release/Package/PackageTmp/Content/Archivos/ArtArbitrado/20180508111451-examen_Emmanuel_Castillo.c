#include <stdio.h>
#include <string.h>
#include <stdlib.h>

typedef struct Node
{
	char character;
	struct Branch *branch;
}Node;

typedef struct Branch
{
	struct Node *node;
	struct Branch *next;
	struct Branch *Parent;
	int root;
}Branch;

typedef struct Stack
{
    char data;
    struct Stack *next;
}Stack;
 
 
void Push(Stack **header,char data);
int Pop(Stack **header);
int EmptyStack(Stack *header);
void NumElements(Stack *header);
void ShowStack(Stack *header);
Node *NewNode(char data);


void newTree(Branch **root);
Node *newNode(char data);
Branch *newBranch();
void PrintTree(Branch *root);  
void PrintTreeNode(Node *node);

void PrintWord(Branch *root,Branch *start); 
int SearchWordIntern(Branch *root,char *word,int len); 
void addChar(Node **node,char ch);
void AddInternal(Branch **root,Branch *Parent,char *word);
Branch *printLv(Branch *Beg,Branch *BPoint,int flag,Branch *root);


//--------------------------------------------------------------------------------------

void addWord(Branch **root,char *word); // Insertar_palabra
int SearchWord(Branch *root,char *word); // buscar_palabr
void PrintChar(Branch *root); // imprimir_todas_las_palabras
void ReversePrint(); // imprimir_las_palabras_al_reves

//--------------------------------------------------------------------------------------


Stack *charStack=NULL;

int main()
{
	Branch *root = NULL;
	newTree(&root);
	
	addWord(&root,"Hoy");
	addWord(&root,"hola");
	addWord(&root,"hay");
	addWord(&root,"Haya");
	addWord(&root,"pez");
	addWord(&root,"paz");
	addWord(&root,"casa");
	addWord(&root,"Emmanuel");
	addWord(&root,"Ponga");
	addWord(&root,"el");
	addWord(&root,"10");
	addWord(&root,"s15001357");

	//int ans = SearchWord(root,"Emmanuel");
	PrintChar(root);
	ReversePrint();
	return 0;
}

void newTree(Branch **root)
{
	Branch *newBran = NULL;
	Node *node = NULL;
	newBran = newBranch();
	node = newNode('*');
	newBran->node = node;
	newBran->root = 1;
	*root = newBran; 
}

Node *newNode(char data)
{
	Node *nNode = (Node*)malloc(sizeof(Node));
	nNode->branch = NULL;
	nNode->character = data;
	return nNode;
}

Branch *newBranch()
{
	Branch *nBran = (Branch*)malloc(sizeof(Branch));
	nBran->node = NULL;
	nBran->next = NULL;
	nBran->Parent = NULL;
	nBran->root = 0;
	return nBran;
}

void PrintTree(Branch *root)
{
	if(root==NULL)
	{
		//printf("Arbol vacio\n");
		return ;
	}

	else
	{
		PrintTreeNode(root->node);
		PrintTree(root->next);
	}
}

void PrintTreeNode(Node *node)
{
	if(node==NULL)
	{
		//printf("Rama vacia:");
		return;
	}

	else
	{
		printf("%c \n",node->character);
		PrintTree(node->branch);
	}
}

void addWord(Branch **root,char *word)
{
	AddInternal(&(*root),NULL,word);
}

void AddInternal(Branch **root,Branch *Parent,char *word)
{
	if(*root == NULL)
	{
		//printf("nueva rama\n");
		*root = newBranch();
		(*root)->Parent = Parent;
		(*root)->node = NULL;
	}

	if((*root)->root==1)
		return AddInternal(&(*root)->node->branch,*root,word);

	if((*root)->node!=NULL)
		if((*root)->node->character==word[0])
		{
			//printf("encontrada [%c]\n",word[0]);
			return AddInternal(&(*root)->node->branch,Parent,word+1);
		}


	if((*root)->node!=NULL)
		if((*root)->node->character!=word[0])
		{
			//printf("Busqueda siguiente hermano\n");
			return AddInternal(&(*root)->next,Parent,word);
		}

	int len = strlen(word);
	int i;

	Branch *aux = *root;

	for(i=0;i<len;i++)
	{
		addChar(&(aux)->node,word[i]);
		(aux)->node->branch = newBranch();
		aux->node->branch->Parent = aux;
		aux = (aux)->node->branch;
	}

	addChar(&(aux)->node,'.');
	(aux)->node->branch = newBranch();
	aux->node->branch->Parent = aux;
}

void addChar(Node **node,char ch)
{	

	*node = newNode(ch);
	//printf("%c \n",ch);
}

int SearchWordIntern(Branch *root,char *word,int len)
{
	if(root==NULL)
		return 0;

	else
	{
		if(root->node->character == '*')
		{
			//printf("-*\n");
			return SearchWordIntern(root->node->branch,word,len);
		}

		if(root->node->character=='.' && len==0)
		{
			//printf("fin . \n");
			return 1;
		}

		while(root!=NULL)
		{
			if(root->node->character==word[0])
			{
				//printf("- [%c] \n",word[0] );
				return SearchWordIntern(root->node->branch,word+1,len-1);
			}

			if(root->node->character!=word[0])
			{
				if(len==0)
				{
					//printf("Limit\n");
					return 0;
				}
				//printf("buscando != %c\n",root->node->character);
				root = root->next;
			}
		}
		//printf("NULL nivel\n");
		return 0;

	}
}
int SearchWord(Branch *root,char *word)
{
	printf("Buscando : %s\n",word);
	return SearchWordIntern(root,word,strlen(word));
}

void PrintWord(Branch *root,Branch *start)
{
	if(root==NULL)
		return;
	
	else
	{
		if(root->node->character == '*')
			PrintWord(root->node->branch,root->node->branch);

		else
		{
			if(root->node->character == '.')
			{
				if(start->next!=NULL)
				{
					printf("\n");
					PrintWord(start->next,start->next);
				}
				return;
			}

			printf("%c",root->node->character);
			PrintWord(root->node->branch,start);
		}
	}
}	

Branch *printLv(Branch *Beg,Branch *BPoint,int flag,Branch *root)
{
	if(Beg==BPoint && Beg!=root)
		return printLv(Beg->next,NULL,flag,root);

	if(Beg->node->character=='.')
		Push(&charStack,' ');
		
	if(Beg->node->character=='.')
		return BPoint;

	printf("%c",Beg->node->character);
	
		Push(&charStack,Beg->node->character);
		

	if(Beg->next!=NULL && flag ==0 && Beg !=root)
	{
		//printf("flag 1\n");
		flag = 1;
		printLv(Beg->node->branch,Beg,flag,root);
	}
	else
		printLv(Beg->node->branch,BPoint,flag,root);
}


void PrintChar(Branch *root)
{
	if(root == NULL)
		return;

	if(root->node->character=='*')
		return PrintChar(root->node->branch);

	Branch *Beg = root;
	Branch *BPoint = root;

	while(root!=NULL)
	{
		int flag = 0;
		Beg = root;
		BPoint = printLv(Beg,BPoint,flag,root);
		printf("\n");
		if(BPoint==NULL)
			root = root->next;
	}

}
 
Node *NewNode(char data)
{
    Stack *nNode=NULL;
    nNode = (Stack*)malloc(sizeof(Stack));
 
    if(nNode==NULL)
    {
        printf("Error de memoria\n");
        return 0;
    }
 
    else
    {
        nNode->data = data;
        nNode->next = NULL;
    }
 
    return nNode;
}
 
void Push(Stack **header,char data)
{
    Stack *nNode;
    nNode = NewNode(data);
 
    if((*header)==NULL)
        *header=nNode;
 
    else
    {
        nNode->next=*header;
        *header=nNode;
    }
}
 
int Pop(Stack **header)
{
    char tempData;
    Stack *DelNode=NULL;
 
    if((*header)==NULL)
        {
            printf("Pila vacia.\n");
            return 0;
        }
 
    else
    {
        tempData=(*header)->data;
        DelNode=*header;
        *header=DelNode->next;
        free(DelNode);
        return tempData;
    }
}
 
 
void ShowStack(Stack *header)
{
    if(header==NULL)
        printf("Pila Vacia.\n");
 
    else
    {
        printf("\n");
        
        while(header!=NULL)
        {
        	if(header->data==' ')
        	{
        		printf("\n");
        		header = header->next;
        		continue;
        	}
        	else
            	printf("%c",header->data);
            	
            header=header->next;
        }
        printf("\n");
    }
}


void ReversePrint()
{
	printf("\nReves: ");
	ShowStack(charStack);
	return;	
}

