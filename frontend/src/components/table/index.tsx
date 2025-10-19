import type { TableProps } from '../../interfaces/table/tableProps'

function Table({ columns, children }: TableProps) {

    return (
        <table className="min-w-full rounded-lg p-7 shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-sm">
            <thead>
                <tr className="text-left">
                    {columns.map((header) => (
                        <th key={header} className="px-4 py-2">{header}</th>
                    ))}
                </tr>
            </thead>
            <tbody>
                {children}
            </tbody>
        </table>
    )
}
export default Table