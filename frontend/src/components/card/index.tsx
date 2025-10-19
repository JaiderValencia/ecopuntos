import type { CardProps } from '../../interfaces/card/cardProps'

function Card({ className, children: children }: CardProps) {
    return (
        <div className={`rounded-lg p-7 shadow-[0_4px_4px_rgba(0,0,0,0.25)] ${className}`}>
            {children}
        </div>
    )
}

export default Card