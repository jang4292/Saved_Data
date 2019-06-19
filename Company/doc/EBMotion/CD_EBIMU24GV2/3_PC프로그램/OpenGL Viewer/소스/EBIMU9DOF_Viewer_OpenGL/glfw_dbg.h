/**
 * @file	GLFW_debug.cpp
 * @author	SiS.
 *
 * @version 1.00.00B.
 *
 * OpenGL (Simple) Framework: Debug routines header file.
 *
 */

#ifndef __GLFW_DBG__
#define __GLFW_DBG__

#ifndef __GLFW__
	#error "Cannot include glfw_dbg.h alone!"
#endif


//////////////////////////////////////////////////////////////////////////
//  #Include section.
//////////////////////////////////////////////////////////////////////////

// System include.
#include <windows.h>
#include <stdlib.h>



//////////////////////////////////////////////////////////////////////////
//  Debug Stuff
//////////////////////////////////////////////////////////////////////////


#ifdef __cplusplus
extern "C" 
{
#endif // __cplusplus


//////////////////////////////////////////////////////////////////////////
//
// NOTE: FOLLOWING FUNCTIONS ARE FOR LIBRARY INTERNAL USAGE ONLY! 
// DO NOT USE THEM DIRECTLY! 
//
extern void __dbg_assert_fail		( const CHAR *lpszFile, INT iLine, const CHAR *lpszExpr );
extern void __dbg_assert_msg_fail	( const CHAR *lpszFile, INT iLine, const CHAR *lpszExpr, const CHAR *lpszMess );
extern void __dbg_assert_ptr_fail	( const CHAR *lpszFile, INT iLine, const CHAR *lpszPtrName );

//////////////////////////////////////////////////////////////////////////


#define _dbg_ptr_wr_access( pAddr, iSize )		( !IsBadWritePtr( (void *)(pAddr), (UINT)(iSize) ) )
#define _dbg_ptr_rd_access( pAddr, iSize )		( !IsBadReadPtr( (const void *)(pAddr), (UINT)(iSize) ) )


#ifdef _DEBUG

#define _dbg_assert( bExpr )					{ if ( !(bExpr) ) __dbg_assert_fail( __FILE__, __LINE__, (#bExpr) ); }
#define _dbg_assert_msg( bExpr, lpszMess ) 		{ if ( !(bExpr) ) __dbg_assert_msg_fail ( __FILE__, __LINE__, (#bExpr), (lpszMess) ); }
#define _dbg_assert_ptr( pPtr )					{ if ( NULL == (pPtr) ) __dbg_assert_ptr_fail ( __FILE__, __LINE__, (#pPtr) ); }
#define _dbg_verify( bExpr )					{ BOOL bRes = (BOOL)(bExpr); if ( !(bExpr) ) __dbg_assert_fail( __FILE__, __LINE__, (#bExpr) ); }
#define _dbg_verify_msg( bExpr, lpszMess )		{ BOOL bRes = (BOOL)(bExpr); if ( !(bExpr) ) __dbg_assert_msg_fail( __FILE__, __LINE__, (#bExpr), (lpszMess) ); }

extern void	_dbg_printf	 ( const PCHAR pFormat, ... );

#else // #ifdef _DEBUG

#define _dbg_assert( bExpr ) 	  				/* */
#define _dbg_assert_msg( bExpr, lpszMess ) 		/* */
#define _dbg_assert_ptr( pPtr )					/* */
#define _dbg_verify( bExpr )					( (bExpr) )
#define _dbg_verify_msg( bExpr, lpszMess )		( (bExpr) )

#define _dbg_printf( lpszFormat )				/* */

#endif // #ifdef _DEBUG
 

#ifdef __cplusplus
}
#endif // __cplusplus


//////////////////////////////////////////////////////////////////////////
//  END of Code.
//////////////////////////////////////////////////////////////////////////

#endif // #ifndef __GLFW_DBG__