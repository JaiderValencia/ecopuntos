function FlexCenter({ children }: { children: React.ReactNode }) {
    return (
        <div className='flex items-center justify-center min-h-[calc(100vh-4rem)]'>
            {children}
        </div>
    )
}

export default FlexCenter