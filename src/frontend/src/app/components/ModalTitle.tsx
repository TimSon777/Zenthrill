import { Text } from '@mantine/core';

const ModalTitle = ({ children }: React.PropsWithChildren) => {
    return (
        <Text fw={700}>{children}</Text>  
    );
}

export default ModalTitle;