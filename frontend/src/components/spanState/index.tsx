function SpanState({ isOpen }: { isOpen: boolean }) {
    return (
        <span className={`inline-block px-2 py-1 rounded text-xs ${isOpen ? 'bg-green-200 text-green-800' : 'bg-gray-300 text-gray-800'}`}>
            {isOpen ? 'Abierto' : 'Cerrado'}
        </span>
    )
}

export default SpanState