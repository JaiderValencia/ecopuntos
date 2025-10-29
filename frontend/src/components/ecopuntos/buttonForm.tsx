function ButtonForm({ children, isActive = false, onClick }: { children: React.ReactNode, isActive: boolean, onClick: (value: string) => void }) {
    return (
        <button
            className={`px-3 py-2 text-sm font-medium rounded-md border-2 ${isActive ? 'bg-blue-500 text-white' : 'bg-white text-black border-[#1976D2] hover:bg-blue-500 hover:text-white'}`}
            type="button"
            onClick={() => onClick(children as string)}
        >{children}</button>
    )

}

export default ButtonForm