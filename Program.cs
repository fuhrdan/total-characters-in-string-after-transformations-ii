//*****************************************************************************
//** 3337. Total Characters in String After Transformations II      leetcode **
//*****************************************************************************

#define MOD 1000000007

int** alloc_matrix(int size)
{
    int** matrix = (int**)malloc(size * sizeof(int*));
    for (int i = 0; i < size; i++)
    {
        matrix[i] = (int*)calloc(size, sizeof(int));
    }
    return matrix;
}

void free_matrix(int** matrix, int size)
{
    for (int i = 0; i < size; i++)
        free(matrix[i]);
    free(matrix);
}

int** get_identity_matrix(int size)
{
    int** I = alloc_matrix(size);
    for (int i = 0; i < size; ++i)
        I[i][i] = 1;
    return I;
}

int** matrix_mult(int** A, int** B, int size)
{
    int** C = alloc_matrix(size);
    for (int i = 0; i < size; i++)
        for (int j = 0; j < size; j++)
            for (int k = 0; k < size; k++)
                C[i][j] = (C[i][j] + (long)A[i][k] * B[k][j]) % MOD;
    return C;
}

int** matrix_pow(int** M, int n, int size)
{
    if (n == 0)
        return get_identity_matrix(size);
    if (n % 2 == 1)
    {
        int** half = matrix_pow(M, n - 1, size);
        int** result = matrix_mult(M, half, size);
        free_matrix(half, size);
        return result;
    }
    else
    {
        int** half = matrix_pow(M, n / 2, size);
        int** result = matrix_mult(half, half, size);
        free_matrix(half, size);
        return result;
    }
}

int** get_transformation_matrix(int* nums, int numsSize)
{
    int** T = alloc_matrix(26);
    for (int i = 0; i < numsSize; i++)
        for (int step = 1; step <= nums[i]; step++)
            T[i][(i + step) % 26]++;
    return T;
}

int lengthAfterTransformations(char* s, int t, int* nums, int numsSize)
{
    int** T = get_transformation_matrix(nums, numsSize);
    int** poweredT = matrix_pow(T, t, 26);
    int count[26] = {0};
    long lengths[26] = {0};
    int retval = 0;

    for (int i = 0; s[i]; i++)
        count[s[i] - 'a']++;

    for (int i = 0; i < 26; i++)
    {
        for (int j = 0; j < 26; j++)
        {
            lengths[j] = (lengths[j] + (long)count[i] * poweredT[i][j]) % MOD;
        }
    }

    for (int i = 0; i < 26; i++)
        retval = (retval + lengths[i]) % MOD;

    free_matrix(T, 26);
    free_matrix(poweredT, 26);
    return retval;
}