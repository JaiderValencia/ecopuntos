function Button({ children, className, type, onClick }: { children: React.ReactNode, className?: string, type?: "button" | "submit" | "reset", onClick?: () => void }) {
    return (
        <button type={type}
            className={`px-6 py-2 font-semibold rounded-md sm:w-auto w-full transition-colors duration-250 ease-in-out ${className}`}
            onClick={onClick}>
            {children}
        </button>
    )
}

export default Button