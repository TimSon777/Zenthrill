'use client';

import { Modal, Textarea, Button, Center, TextInput } from '@mantine/core';
import { IFragment, IUpdateFragmentRequest } from '@/app/types';
import { useForm } from '@mantine/form';
import ModalTitle from '@/app/components/ModalTitle';
import updateFragment from '@/app/api/updateFragment';
import { useDisclosure } from '@mantine/hooks';
import markFragmentEntrypoint from '@/app/api/markFragmentEntrypoint';
import AddFragmentModal from './AddFragmentModal';

interface IProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    fragment: IFragment;
};

interface IUpdateProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    fragment: IFragment; 
}

const UpdateModal = ({ opened, close, storyInfoVersionId, onStoryChanged, fragment }: IUpdateProps) => {

    const form = useForm<IUpdateFragmentRequest>({
        initialValues: {
            name: fragment.name,
            storyInfoVersionId,
            body: fragment.body,
            fragmentId: fragment.id,
        },
    });
    
    const onClick = async () => {
        await updateFragment(form.values);
        onStoryChanged();
        close();
    };
    
    return (
      <Modal
          opened={opened}
          onClose={close}
          title={<ModalTitle>Обновление фрагмента</ModalTitle>}>
          
          <TextInput
              label="Название фрагмента"
              placeholder="Введите название фрагмента"
              mt="sm"
              {...form.getInputProps('name')}
          />
          
          <Textarea
              label="Текст фрагмента"
              placeholder="Введите текст фрагмента"
              autosize
              minRows={3}
              mt="sm"
              {...form.getInputProps('body')}
          />
          <Center>
              <Button onClick={onClick}>
                  Обновить
              </Button>
          </Center>
      </Modal>  
    );
}

const UpdateFragmentModal = ({ opened, close, storyInfoVersionId, onStoryChanged, fragment }: IProps) => {
    const [updateFragmentModalOpened, { open: openUpdateFragmentModal, close: closeUpdateFragmentModal }] = useDisclosure();
    const [addFragmentModalOpened, { open: openAddFragmentModal, close: closeAddFragmentModal }] = useDisclosure();
    
    const onMarkFragmentEntrypoint = async () => {
        await markFragmentEntrypoint(fragment.id, storyInfoVersionId);
        onStoryChanged();
    };
    
    return (
        <Modal
            opened={opened}
            onClose={close}
            title={<ModalTitle>Редактирование фрагмента</ModalTitle>}
        >
            <UpdateModal 
                opened={updateFragmentModalOpened}
                close={closeUpdateFragmentModal}
                storyInfoVersionId={storyInfoVersionId}
                onStoryChanged={onStoryChanged}
                fragment={fragment} />

            <AddFragmentModal
                opened={addFragmentModalOpened}
                close={closeAddFragmentModal}
                fromFragmentId={fragment.id}
                storyInfoVersionId={storyInfoVersionId}
                onStoryChanged={onStoryChanged}
            />
            
            <Center>
                <Button mr={'20px'} onClick={openUpdateFragmentModal}>Обновить</Button>
                <Button mr={'20px'} onClick={openAddFragmentModal}>Добавить</Button>
                <Button onClick={onMarkFragmentEntrypoint}>Сделать входной точкой</Button>
            </Center>
        </Modal>
    );
};

export default UpdateFragmentModal;